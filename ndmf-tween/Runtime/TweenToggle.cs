using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exor455.NdmfTween
{
    public enum TweenTargetType
    {
        Blendshape,
        MaterialFloat,
        MaterialColor,
        Transform
    }

    public enum TransformProperty
    {
        LocalPosition,
        LocalRotationEuler,
        LocalScale
    }

    public enum TweenTransitionMode
    {
        /// <summary>CrossFade when easing is Linear and no per-track delay/curve, otherwise BakedClips.</summary>
        Auto,

        /// <summary>Two 1-frame states blended by transition duration. Linear only, perfectly reversible mid-tween.</summary>
        CrossFade,

        /// <summary>Eased tween clips baked at build time. Mid-tween reversal approximated by Cancel Blend.</summary>
        BakedClips
    }

    [Serializable]
    public class TweenTrack
    {
        public TweenTargetType type = TweenTargetType.Blendshape;

        public Renderer renderer;
        public Transform targetTransform;
        public TransformProperty transformProperty = TransformProperty.LocalPosition;

        /// <summary>
        /// Blendshape name, or material property name (e.g. "_DissolveAlpha").
        /// Material vector components may be addressed as "_Prop.x" etc.
        /// </summary>
        public string propertyName = "";

        public float startFloat;
        public float endFloat = 1f;

        [ColorUsage(true, true)] public Color startColor = Color.white;
        [ColorUsage(true, true)] public Color endColor = Color.white;

        public Vector3 startVector;
        public Vector3 endVector;

        /// <summary>Start delay in seconds. Forces BakedClips mode when > 0.</summary>
        [Min(0f)] public float delay;

        public bool overrideCurve;
        public AnimationCurve curveOverride = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    }

    /// <summary>
    /// Generates an AnimationClip + Animator layer at NDMF build time that tweens the
    /// configured properties between start and end values over a duration, toggled by a
    /// Bool parameter. Menu/parameter integration is delegated to Modular Avatar
    /// (a Merge Animator and Parameters entry are generated in the Generating phase).
    /// </summary>
    [AddComponentMenu("NDMF Tween/NT Tween Toggle")]
    [HelpURL("https://github.com/exor455/VRC_Unity_tutorial/tree/main/ndmf-tween")]
    public class TweenToggle : MonoBehaviour
#if NT_VRCSDK3A
        , VRC.SDKBase.IEditorOnly
#endif
    {
        [Tooltip("Bool parameter that triggers the tween. Pair it with an MA Menu Item (Toggle). Empty = auto (\"Tween/<object name>\").")]
        public string parameterName = "";

        public bool defaultOn;
        public bool saved = true;
        public bool synced = true;

        [Min(0.01f)] public float duration = 1f;

        [Tooltip("Duration for the OFF direction. Negative = same as Duration.")]
        public float reverseDuration = -1f;

        public EasingKind easing = EasingKind.Linear;
        public AnimationCurve customCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public TweenTransitionMode transitionMode = TweenTransitionMode.Auto;

        [Tooltip("BakedClips only: crossfade seconds used when the toggle is reversed mid-tween.")]
        [Min(0f)] public float cancelBlend = 0.15f;

        [Tooltip("If a material property is missing, remap to a uniquely matching renamed property (Poiyomi locked shaders).")]
        public bool poiyomiAutoRemap = true;

        public List<TweenTrack> tracks = new List<TweenTrack>();

        public string EffectiveParameterName =>
            string.IsNullOrWhiteSpace(parameterName) ? $"Tween/{gameObject.name}" : parameterName.Trim();

        public float EffectiveReverseDuration => reverseDuration > 0f ? reverseDuration : duration;

        public bool RequiresBakedClips()
        {
            if (easing != EasingKind.Linear) return true;
            foreach (var track in tracks)
            {
                if (track == null) continue;
                if (track.delay > 0f || track.overrideCurve) return true;
            }

            return false;
        }
    }
}
