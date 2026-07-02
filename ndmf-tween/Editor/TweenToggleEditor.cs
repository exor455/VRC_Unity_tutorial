using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Exor455.NdmfTween.Editor
{
    [CustomEditor(typeof(TweenToggle))]
    [CanEditMultipleObjects]
    public class TweenToggleEditor : UnityEditor.Editor
    {
        private SerializedProperty _parameterName;
        private SerializedProperty _defaultOn;
        private SerializedProperty _saved;
        private SerializedProperty _synced;
        private SerializedProperty _duration;
        private SerializedProperty _reverseDuration;
        private SerializedProperty _easing;
        private SerializedProperty _customCurve;
        private SerializedProperty _transitionMode;
        private SerializedProperty _cancelBlend;
        private SerializedProperty _poiyomiAutoRemap;
        private SerializedProperty _tracks;

        private void OnEnable()
        {
            _parameterName = serializedObject.FindProperty(nameof(TweenToggle.parameterName));
            _defaultOn = serializedObject.FindProperty(nameof(TweenToggle.defaultOn));
            _saved = serializedObject.FindProperty(nameof(TweenToggle.saved));
            _synced = serializedObject.FindProperty(nameof(TweenToggle.synced));
            _duration = serializedObject.FindProperty(nameof(TweenToggle.duration));
            _reverseDuration = serializedObject.FindProperty(nameof(TweenToggle.reverseDuration));
            _easing = serializedObject.FindProperty(nameof(TweenToggle.easing));
            _customCurve = serializedObject.FindProperty(nameof(TweenToggle.customCurve));
            _transitionMode = serializedObject.FindProperty(nameof(TweenToggle.transitionMode));
            _cancelBlend = serializedObject.FindProperty(nameof(TweenToggle.cancelBlend));
            _poiyomiAutoRemap = serializedObject.FindProperty(nameof(TweenToggle.poiyomiAutoRemap));
            _tracks = serializedObject.FindProperty(nameof(TweenToggle.tracks));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var toggle = (TweenToggle)target;

            EditorGUILayout.LabelField("Trigger", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_parameterName, new GUIContent("Parameter Name"));
            if (string.IsNullOrWhiteSpace(_parameterName.stringValue))
            {
                EditorGUILayout.HelpBox($"パラメータ名が空のため \"{toggle.EffectiveParameterName}\" が使われます。MA Menu Item (Toggle) の parameter に同じ名前を設定してください。", MessageType.Info);
            }

            EditorGUILayout.PropertyField(_defaultOn);
            EditorGUILayout.PropertyField(_saved);
            EditorGUILayout.PropertyField(_synced);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tween", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_duration, new GUIContent("Duration (s)"));
            EditorGUILayout.PropertyField(_reverseDuration, new GUIContent("Reverse Duration (s)"));
            EditorGUILayout.PropertyField(_easing);
            if ((EasingKind)_easing.enumValueIndex == EasingKind.Custom)
            {
                EditorGUILayout.PropertyField(_customCurve, new GUIContent("Custom Curve (0-1)"));
            }

            EditorGUILayout.PropertyField(_transitionMode);
            var mode = (TweenTransitionMode)_transitionMode.enumValueIndex;
            bool baked = mode == TweenTransitionMode.BakedClips ||
                         (mode == TweenTransitionMode.Auto && toggle.RequiresBakedClips());
            if (baked)
            {
                EditorGUILayout.PropertyField(_cancelBlend, new GUIContent("Cancel Blend (s)"));
                EditorGUILayout.HelpBox("BakedClips: イージングを焼き込んだクリップを生成します。トゥイーン中の反転はCancel Blend秒のクロスフェードで近似されます。", MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox("CrossFade: 2ステート間の遷移時間で補間します(線形のみ・反転に強い)。", MessageType.None);
            }

            if (mode == TweenTransitionMode.CrossFade && toggle.RequiresBakedClips())
            {
                EditorGUILayout.HelpBox("CrossFadeモードではイージング/Delay/カーブ上書きは無視されます。", MessageType.Warning);
            }

            EditorGUILayout.PropertyField(_poiyomiAutoRemap, new GUIContent("Poiyomi Auto Remap"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Tracks ({_tracks.arraySize})", EditorStyles.boldLabel);

            int removeIndex = -1;
            for (int i = 0; i < _tracks.arraySize; i++)
            {
                var element = _tracks.GetArrayElementAtIndex(i);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("type"), GUIContent.none);
                        if (GUILayout.Button("✕", GUILayout.Width(24))) removeIndex = i;
                    }

                    DrawTrack(element, toggle, i);
                }
            }

            if (removeIndex >= 0) _tracks.DeleteArrayElementAtIndex(removeIndex);

            if (GUILayout.Button("Add Track"))
            {
                _tracks.arraySize++;
            }

            serializedObject.ApplyModifiedProperties();

            foreach (var warning in Validate(toggle))
            {
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
            }
        }

        private void DrawTrack(SerializedProperty element, TweenToggle toggle, int index)
        {
            var type = (TweenTargetType)element.FindPropertyRelative("type").enumValueIndex;
            var propertyName = element.FindPropertyRelative("propertyName");

            switch (type)
            {
                case TweenTargetType.Blendshape:
                case TweenTargetType.MaterialFloat:
                case TweenTargetType.MaterialColor:
                {
                    var rendererProp = element.FindPropertyRelative("renderer");
                    EditorGUILayout.PropertyField(rendererProp, new GUIContent("Renderer"));

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.PropertyField(propertyName, new GUIContent(type == TweenTargetType.Blendshape ? "Blendshape" : "Property"));
                        if (GUILayout.Button("▾", GUILayout.Width(24)))
                        {
                            ShowPropertyPicker(type, rendererProp.objectReferenceValue as Renderer, propertyName);
                        }
                    }

                    if (type == TweenTargetType.MaterialColor)
                    {
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("startColor"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("endColor"));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("startFloat"), new GUIContent("Start Value"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("endFloat"), new GUIContent("End Value"));
                        if (type == TweenTargetType.Blendshape && GUILayout.Button("Set Start From Current", EditorStyles.miniButton))
                        {
                            SetStartFromCurrent(element, toggle);
                        }
                    }

                    break;
                }

                case TweenTargetType.Transform:
                {
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("targetTransform"), new GUIContent("Transform"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("transformProperty"), new GUIContent("Property"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("startVector"), new GUIContent("Start Value"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("endVector"), new GUIContent("End Value"));
                    break;
                }
            }

            EditorGUILayout.PropertyField(element.FindPropertyRelative("delay"), new GUIContent("Delay (s)"));
            var overrideCurve = element.FindPropertyRelative("overrideCurve");
            EditorGUILayout.PropertyField(overrideCurve, new GUIContent("Curve Override"));
            if (overrideCurve.boolValue)
            {
                EditorGUILayout.PropertyField(element.FindPropertyRelative("curveOverride"), new GUIContent("Curve (0-1)"));
            }
        }

        private void SetStartFromCurrent(SerializedProperty element, TweenToggle toggle)
        {
            var renderer = element.FindPropertyRelative("renderer").objectReferenceValue as SkinnedMeshRenderer;
            var shapeName = element.FindPropertyRelative("propertyName").stringValue;
            if (renderer == null || renderer.sharedMesh == null) return;
            int idx = renderer.sharedMesh.GetBlendShapeIndex(shapeName);
            if (idx < 0) return;
            element.FindPropertyRelative("startFloat").floatValue = renderer.GetBlendShapeWeight(idx);
        }

        private void ShowPropertyPicker(TweenTargetType type, Renderer renderer, SerializedProperty propertyName)
        {
            if (renderer == null) return;
            var menu = new GenericMenu();
            var serialized = propertyName.serializedObject;
            var path = propertyName.propertyPath;

            void AddItem(string label, string value)
            {
                menu.AddItem(new GUIContent(label), false, () =>
                {
                    serialized.Update();
                    serialized.FindProperty(path).stringValue = value;
                    serialized.ApplyModifiedProperties();
                });
            }

            if (type == TweenTargetType.Blendshape)
            {
                if (renderer is SkinnedMeshRenderer smr && smr.sharedMesh != null)
                {
                    for (int i = 0; i < smr.sharedMesh.blendShapeCount; i++)
                    {
                        var name = smr.sharedMesh.GetBlendShapeName(i);
                        AddItem(name.Replace('/', '∕'), name);
                    }
                }
            }
            else
            {
                foreach (var mat in renderer.sharedMaterials.Where(m => m != null && m.shader != null).Distinct())
                {
                    var shader = mat.shader;
                    int count = shader.GetPropertyCount();
                    for (int i = 0; i < count; i++)
                    {
                        var propType = shader.GetPropertyType(i);
                        bool match = type == TweenTargetType.MaterialColor
                            ? propType == ShaderPropertyType.Color
                            : propType == ShaderPropertyType.Float || propType == ShaderPropertyType.Range;
                        if (!match) continue;
                        var name = shader.GetPropertyName(i);
                        AddItem($"{mat.name.Replace('/', '∕')}/{name}", name);
                    }
                }
            }

            if (menu.GetItemCount() == 0)
            {
                menu.AddDisabledItem(new GUIContent("(候補なし)"));
            }

            menu.ShowAsContext();
        }

        private IEnumerable<string> Validate(TweenToggle toggle)
        {
            var warnings = new List<string>();
            if (toggle.tracks.Count == 0)
            {
                warnings.Add("トラックがありません。ビルド時に何も生成されません。");
            }

            for (int i = 0; i < toggle.tracks.Count; i++)
            {
                var track = toggle.tracks[i];
                if (track == null) continue;
                switch (track.type)
                {
                    case TweenTargetType.Blendshape:
                        if (!(track.renderer is SkinnedMeshRenderer))
                            warnings.Add($"Track {i + 1}: SkinnedMeshRendererを指定してください。");
                        else if (string.IsNullOrWhiteSpace(track.propertyName))
                            warnings.Add($"Track {i + 1}: Blendshape名が未設定です。");
                        break;

                    case TweenTargetType.MaterialFloat:
                    case TweenTargetType.MaterialColor:
                        if (track.renderer == null)
                        {
                            warnings.Add($"Track {i + 1}: Rendererが未設定です。");
                        }
                        else if (string.IsNullOrWhiteSpace(track.propertyName))
                        {
                            warnings.Add($"Track {i + 1}: プロパティ名が未設定です。");
                        }
                        else
                        {
                            var result = PoiyomiPropertyResolver.Resolve(track.renderer, track.propertyName, toggle.poiyomiAutoRemap);
                            if (result.kind == PoiyomiPropertyResolver.ResultKind.NotFound)
                                warnings.Add($"Track {i + 1}: {result.message}");
                            else if (result.kind == PoiyomiPropertyResolver.ResultKind.Remapped)
                                warnings.Add($"Track {i + 1}: {result.message}");

                            var animatedWarning = PoiyomiPropertyResolver.CheckPoiyomiAnimatedTag(track.renderer, result.resolvedName);
                            if (animatedWarning != null) warnings.Add($"Track {i + 1}: {animatedWarning}");
                        }

                        break;

                    case TweenTargetType.Transform:
                        if (track.targetTransform == null)
                            warnings.Add($"Track {i + 1}: Transformが未設定です。");
                        break;
                }
            }

            return warnings;
        }
    }
}
