#if NT_VRCSDK3A
using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

[assembly: ExportsPlugin(typeof(Exor455.NdmfTween.Editor.TweenPlugin))]

namespace Exor455.NdmfTween.Editor
{
    /// <summary>
    /// Runs in the Generating phase: converts each TweenToggle into an AnimatorController
    /// plus MA Merge Animator / MA Parameters components, then removes the TweenToggle.
    /// Merging into FX, write-defaults matching, path remapping after Merge Armature and
    /// MMD handling are all delegated to Modular Avatar (which runs in Transforming).
    /// </summary>
    public class TweenPlugin : Plugin<TweenPlugin>
    {
        public override string QualifiedName => "dev.exor455.ndmf-tween";
        public override string DisplayName => "NDMF Tween Generator";

        protected override void Configure()
        {
            InPhase(BuildPhase.Generating)
                .Run("Generate tween animators", ctx => TweenProcessor.Process(ctx));
        }
    }

    internal static class TweenProcessor
    {
        internal static void Process(BuildContext ctx)
        {
            var toggles = ctx.AvatarRootObject.GetComponentsInChildren<TweenToggle>(true);
            foreach (var toggle in toggles)
            {
                try
                {
                    ProcessOne(ctx, toggle);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(
                        $"[NDMF Tween] '{toggle.name}' の処理中に例外が発生したためスキップしました: {e}",
                        toggle.gameObject);
                }
                finally
                {
                    Object.DestroyImmediate(toggle);
                }
            }
        }

        private static void ProcessOne(BuildContext ctx, TweenToggle toggle)
        {
            var parameterName = toggle.EffectiveParameterName;
            var bindings = ResolveBindings(ctx.AvatarRootTransform, toggle);
            if (bindings.Count == 0)
            {
                Debug.LogWarning(
                    $"[NDMF Tween] '{toggle.name}' に有効なトラックがないため何も生成しませんでした。",
                    toggle.gameObject);
                return;
            }

            bool useCrossFade;
            switch (toggle.transitionMode)
            {
                case TweenTransitionMode.CrossFade:
                    useCrossFade = true;
                    if (toggle.RequiresBakedClips())
                    {
                        Debug.LogWarning(
                            $"[NDMF Tween] '{toggle.name}': CrossFadeモードではイージング/Delay/カーブ上書きは無視されます(線形補間)。",
                            toggle.gameObject);
                    }

                    break;
                case TweenTransitionMode.BakedClips:
                    useCrossFade = false;
                    break;
                default:
                    useCrossFade = !toggle.RequiresBakedClips();
                    break;
            }

            var input = new TweenAnimatorBuilder.BuildInput
            {
                parameterName = parameterName,
                defaultOn = toggle.defaultOn,
                duration = Mathf.Max(0.01f, toggle.duration),
                reverseDuration = Mathf.Max(0.01f, toggle.EffectiveReverseDuration),
                cancelBlend = toggle.cancelBlend,
                useCrossFade = useCrossFade,
                bindings = bindings
            };

            var assetSaver = ctx.AssetSaver;
            var controller = TweenAnimatorBuilder.Build(
                input,
                assetSaver != null ? new System.Action<Object>(o => assetSaver.SaveAsset(o)) : null);

            var host = new GameObject($"NT Tween ({TweenAnimatorBuilder.Sanitize(parameterName)})");
            host.transform.SetParent(toggle.transform, false);

            var merge = host.AddComponent<ModularAvatarMergeAnimator>();
            merge.animator = controller;
            merge.layerType = VRCAvatarDescriptor.AnimLayerType.FX;
            merge.pathMode = MergeAnimatorPathMode.Absolute;
            merge.matchAvatarWriteDefaults = true;
            merge.deleteAttachedAnimator = false;

            var maParameters = host.AddComponent<ModularAvatarParameters>();
            maParameters.parameters.Add(new ParameterConfig
            {
                nameOrPrefix = parameterName,
                syncType = ParameterSyncType.Bool,
                defaultValue = toggle.defaultOn ? 1f : 0f,
                hasExplicitDefaultValue = true,
                saved = toggle.saved,
                localOnly = !toggle.synced
            });
        }

        private static List<TweenAnimatorBuilder.FloatBinding> ResolveBindings(Transform avatarRoot, TweenToggle toggle)
        {
            var bindings = new List<TweenAnimatorBuilder.FloatBinding>();

            foreach (var track in toggle.tracks)
            {
                if (track == null) continue;
                var easing = track.overrideCurve ? EasingKind.Custom : toggle.easing;
                var curve = track.overrideCurve ? track.curveOverride : toggle.customCurve;

                switch (track.type)
                {
                    case TweenTargetType.Blendshape:
                    {
                        if (!(track.renderer is SkinnedMeshRenderer smr) || string.IsNullOrWhiteSpace(track.propertyName))
                        {
                            WarnTrack(toggle, "Blendshapeトラックの対象(SkinnedMeshRenderer)またはBlendshape名が未設定です。");
                            continue;
                        }

                        var path = RelativePath(avatarRoot, smr.transform);
                        if (path == null)
                        {
                            WarnTrack(toggle, $"'{smr.name}' がアバター配下にありません。");
                            continue;
                        }

                        if (smr.sharedMesh != null && smr.sharedMesh.GetBlendShapeIndex(track.propertyName) < 0)
                        {
                            WarnTrack(toggle, $"Blendshape '{track.propertyName}' が '{smr.name}' に存在しません(そのまま生成します)。");
                        }

                        bindings.Add(new TweenAnimatorBuilder.FloatBinding
                        {
                            path = path,
                            bindingType = typeof(SkinnedMeshRenderer),
                            propertyName = "blendShape." + track.propertyName.Trim(),
                            start = track.startFloat,
                            end = track.endFloat,
                            delay = track.delay,
                            easing = easing,
                            customCurve = curve
                        });
                        break;
                    }

                    case TweenTargetType.MaterialFloat:
                    case TweenTargetType.MaterialColor:
                    {
                        if (track.renderer == null || string.IsNullOrWhiteSpace(track.propertyName))
                        {
                            WarnTrack(toggle, "Materialトラックの対象Rendererまたはプロパティ名が未設定です。");
                            continue;
                        }

                        var path = RelativePath(avatarRoot, track.renderer.transform);
                        if (path == null)
                        {
                            WarnTrack(toggle, $"'{track.renderer.name}' がアバター配下にありません。");
                            continue;
                        }

                        var resolved = PoiyomiPropertyResolver.Resolve(
                            track.renderer, track.propertyName, toggle.poiyomiAutoRemap);
                        if (resolved.kind == PoiyomiPropertyResolver.ResultKind.NotFound)
                        {
                            WarnTrack(toggle, resolved.message + "(そのまま生成します)");
                            resolved.resolvedName = track.propertyName.Trim();
                        }
                        else if (resolved.kind == PoiyomiPropertyResolver.ResultKind.Remapped)
                        {
                            Debug.Log($"[NDMF Tween] '{toggle.name}': {resolved.message}", toggle.gameObject);
                        }

                        var animatedWarning = PoiyomiPropertyResolver.CheckPoiyomiAnimatedTag(
                            track.renderer, resolved.resolvedName);
                        if (animatedWarning != null)
                        {
                            WarnTrack(toggle, animatedWarning);
                        }

                        var rendererType = track.renderer.GetType();
                        if (track.type == TweenTargetType.MaterialFloat)
                        {
                            bindings.Add(new TweenAnimatorBuilder.FloatBinding
                            {
                                path = path,
                                bindingType = rendererType,
                                propertyName = "material." + resolved.resolvedName,
                                start = track.startFloat,
                                end = track.endFloat,
                                delay = track.delay,
                                easing = easing,
                                customCurve = curve
                            });
                        }
                        else
                        {
                            PoiyomiPropertyResolver.SplitComponentSuffix(resolved.resolvedName, out var baseName, out _);
                            var channels = new[] { ".r", ".g", ".b", ".a" };
                            for (int i = 0; i < 4; i++)
                            {
                                bindings.Add(new TweenAnimatorBuilder.FloatBinding
                                {
                                    path = path,
                                    bindingType = rendererType,
                                    propertyName = "material." + baseName + channels[i],
                                    start = track.startColor[i],
                                    end = track.endColor[i],
                                    delay = track.delay,
                                    easing = easing,
                                    customCurve = curve
                                });
                            }
                        }

                        break;
                    }

                    case TweenTargetType.Transform:
                    {
                        if (track.targetTransform == null)
                        {
                            WarnTrack(toggle, "Transformトラックの対象が未設定です。");
                            continue;
                        }

                        var path = RelativePath(avatarRoot, track.targetTransform);
                        if (path == null)
                        {
                            WarnTrack(toggle, $"'{track.targetTransform.name}' がアバター配下にありません。");
                            continue;
                        }

                        string prefix;
                        switch (track.transformProperty)
                        {
                            case TransformProperty.LocalRotationEuler:
                                prefix = "localEulerAnglesRaw";
                                break;
                            case TransformProperty.LocalScale:
                                prefix = "m_LocalScale";
                                break;
                            default:
                                prefix = "m_LocalPosition";
                                break;
                        }

                        var axes = new[] { ".x", ".y", ".z" };
                        for (int i = 0; i < 3; i++)
                        {
                            bindings.Add(new TweenAnimatorBuilder.FloatBinding
                            {
                                path = path,
                                bindingType = typeof(Transform),
                                propertyName = prefix + axes[i],
                                start = track.startVector[i],
                                end = track.endVector[i],
                                delay = track.delay,
                                easing = easing,
                                customCurve = curve
                            });
                        }

                        break;
                    }
                }
            }

            return bindings;
        }

        private static void WarnTrack(TweenToggle toggle, string message)
        {
            Debug.LogWarning($"[NDMF Tween] '{toggle.name}': {message}", toggle.gameObject);
        }

        internal static string RelativePath(Transform root, Transform target)
        {
            var parts = new List<string>();
            var current = target;
            while (current != null && current != root)
            {
                parts.Add(current.name);
                current = current.parent;
            }

            if (current == null) return null;
            parts.Reverse();
            return string.Join("/", parts);
        }
    }
}
#endif
