using UnityEngine;

namespace Exor455.NdmfTween
{
    public enum EasingKind
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InOutCubic,
        InOutSine,
        Custom
    }

    public static class Easing
    {
        /// <summary>
        /// Evaluates the normalized easing function (t: 0-1 -> 0-1).
        /// </summary>
        public static float Evaluate(EasingKind kind, AnimationCurve custom, float t)
        {
            t = Mathf.Clamp01(t);
            switch (kind)
            {
                case EasingKind.Linear:
                    return t;
                case EasingKind.InQuad:
                    return t * t;
                case EasingKind.OutQuad:
                    return 1f - (1f - t) * (1f - t);
                case EasingKind.InOutQuad:
                    return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
                case EasingKind.InOutCubic:
                    return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
                case EasingKind.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
                case EasingKind.Custom:
                    return custom != null ? custom.Evaluate(t) : t;
                default:
                    return t;
            }
        }
    }
}
