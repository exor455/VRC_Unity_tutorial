#if NT_VRCSDK3A
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Exor455.NdmfTween.Editor
{
    /// <summary>
    /// Builds the AnimatorController (single layer) and AnimationClips for one TweenToggle.
    /// All bindings are flattened to float curves addressed by avatar-root-relative paths
    /// (the generated controller is merged with MA Merge Animator in Absolute path mode).
    /// </summary>
    internal static class TweenAnimatorBuilder
    {
        internal class FloatBinding
        {
            public string path;
            public Type bindingType;
            public string propertyName;
            public float start;
            public float end;
            public float delay;
            public EasingKind easing;
            public AnimationCurve customCurve;
        }

        internal class BuildInput
        {
            public string parameterName;
            public bool defaultOn;
            public float duration;
            public float reverseDuration;
            public float cancelBlend;
            public bool useCrossFade;
            public List<FloatBinding> bindings = new List<FloatBinding>();
        }

        internal static AnimatorController Build(BuildInput input, Action<UnityEngine.Object> saveAsset)
        {
            var param = input.parameterName;

            var controller = new AnimatorController { name = $"NT Tween {Sanitize(param)}" };
            controller.AddParameter(new AnimatorControllerParameter
            {
                name = param,
                type = AnimatorControllerParameterType.Bool,
                defaultBool = input.defaultOn
            });

            controller.AddLayer($"Tween/{param}");
            var layers = controller.layers;
            layers[layers.Length - 1].defaultWeight = 1f;
            controller.layers = layers;
            var sm = controller.layers[controller.layers.Length - 1].stateMachine;

            var offClip = MakeHoldClip($"NT {Sanitize(param)} Off", input.bindings, useStart: true);
            var onClip = MakeHoldClip($"NT {Sanitize(param)} On", input.bindings, useStart: false);

            var offState = sm.AddState("Off", new Vector3(240, 120));
            offState.motion = offClip;
            offState.writeDefaultValues = false;

            var onState = sm.AddState("On", new Vector3(240, 320));
            onState.motion = onClip;
            onState.writeDefaultValues = false;

            var generated = new List<UnityEngine.Object> { offClip, onClip };

            if (input.useCrossFade)
            {
                var toOn = offState.AddTransition(onState);
                ConfigureImmediateTransition(toOn, input.duration);
                toOn.interruptionSource = TransitionInterruptionSource.SourceThenDestination;
                toOn.AddCondition(AnimatorConditionMode.If, 0, param);

                var toOff = onState.AddTransition(offState);
                ConfigureImmediateTransition(toOff, input.reverseDuration);
                toOff.interruptionSource = TransitionInterruptionSource.SourceThenDestination;
                toOff.AddCondition(AnimatorConditionMode.IfNot, 0, param);
            }
            else
            {
                var fwdClip = MakeTweenClip($"NT {Sanitize(param)} In", input.bindings, forward: true, input.duration);
                var revClip = MakeTweenClip($"NT {Sanitize(param)} Out", input.bindings, forward: false, input.reverseDuration);
                generated.Add(fwdClip);
                generated.Add(revClip);

                var tweenIn = sm.AddState("TweenIn", new Vector3(480, 120));
                tweenIn.motion = fwdClip;
                tweenIn.writeDefaultValues = false;

                var tweenOut = sm.AddState("TweenOut", new Vector3(480, 320));
                tweenOut.motion = revClip;
                tweenOut.writeDefaultValues = false;

                var offToIn = offState.AddTransition(tweenIn);
                ConfigureImmediateTransition(offToIn, 0f);
                offToIn.AddCondition(AnimatorConditionMode.If, 0, param);

                var inToOn = tweenIn.AddTransition(onState);
                inToOn.hasExitTime = true;
                inToOn.exitTime = 1f;
                inToOn.hasFixedDuration = true;
                inToOn.duration = 0f;

                var onToOut = onState.AddTransition(tweenOut);
                ConfigureImmediateTransition(onToOut, 0f);
                onToOut.AddCondition(AnimatorConditionMode.IfNot, 0, param);

                var outToOff = tweenOut.AddTransition(offState);
                outToOff.hasExitTime = true;
                outToOff.exitTime = 1f;
                outToOff.hasFixedDuration = true;
                outToOff.duration = 0f;

                // Mid-tween reversal: crossfade between the two tween states.
                var inToOut = tweenIn.AddTransition(tweenOut);
                ConfigureImmediateTransition(inToOut, input.cancelBlend);
                inToOut.AddCondition(AnimatorConditionMode.IfNot, 0, param);

                var outToIn = tweenOut.AddTransition(tweenIn);
                ConfigureImmediateTransition(outToIn, input.cancelBlend);
                outToIn.AddCondition(AnimatorConditionMode.If, 0, param);
            }

            sm.defaultState = input.defaultOn ? onState : offState;

            if (saveAsset != null)
            {
                foreach (var clip in generated) saveAsset(clip);
                foreach (var state in sm.states) saveAsset(state.state);
                foreach (var state in sm.states)
                foreach (var transition in state.state.transitions)
                    saveAsset(transition);
                saveAsset(sm);
                saveAsset(controller);
            }

            return controller;
        }

        private static void ConfigureImmediateTransition(AnimatorStateTransition transition, float durationSeconds)
        {
            transition.hasExitTime = false;
            transition.exitTime = 0f;
            transition.hasFixedDuration = true;
            transition.duration = durationSeconds;
            transition.offset = 0f;
        }

        private static AnimationClip MakeHoldClip(string name, List<FloatBinding> bindings, bool useStart)
        {
            var clip = new AnimationClip { name = name };
            foreach (var binding in bindings)
            {
                float value = useStart ? binding.start : binding.end;
                var curve = new AnimationCurve(new Keyframe(0f, value), new Keyframe(1f / 60f, value));
                SetCurve(clip, binding, curve);
            }

            return clip;
        }

        private static AnimationClip MakeTweenClip(string name, List<FloatBinding> bindings, bool forward, float duration)
        {
            var clip = new AnimationClip { name = name };
            float clipLength = bindings.Max(b => b.delay) + duration;

            foreach (var binding in bindings)
            {
                float from = forward ? binding.start : binding.end;
                float to = forward ? binding.end : binding.start;
                var curve = BuildEasedCurve(from, to, binding.delay, duration, binding.easing, binding.customCurve, clipLength);
                SetCurve(clip, binding, curve);
            }

            return clip;
        }

        private static void SetCurve(AnimationClip clip, FloatBinding binding, AnimationCurve curve)
        {
            var editorBinding = EditorCurveBinding.FloatCurve(binding.path, binding.bindingType, binding.propertyName);
            AnimationUtility.SetEditorCurve(clip, editorBinding, curve);
        }

        internal static AnimationCurve BuildEasedCurve(
            float from, float to, float delay, float duration,
            EasingKind easing, AnimationCurve customCurve, float clipLength)
        {
            var keys = new List<Keyframe>();
            if (delay > 0f)
            {
                keys.Add(new Keyframe(0f, from, 0f, 0f));
            }

            if (easing == EasingKind.Linear)
            {
                float slope = (to - from) / duration;
                keys.Add(new Keyframe(delay, from, 0f, slope));
                keys.Add(new Keyframe(delay + duration, to, slope, 0f));
            }
            else
            {
                const int samples = 24;
                for (int i = 0; i <= samples; i++)
                {
                    float t = i / (float)samples;
                    float value = from + (to - from) * Easing.Evaluate(easing, customCurve, t);
                    keys.Add(new Keyframe(delay + t * duration, value));
                }
            }

            if (delay + duration < clipLength - 1e-4f)
            {
                keys.Add(new Keyframe(clipLength, to, 0f, 0f));
            }

            var curve = new AnimationCurve(keys.ToArray());
            if (easing != EasingKind.Linear)
            {
                for (int i = 0; i < curve.length; i++) curve.SmoothTangents(i, 0f);
            }

            return curve;
        }

        internal static string Sanitize(string name)
        {
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return name;
        }
    }
}
#endif
