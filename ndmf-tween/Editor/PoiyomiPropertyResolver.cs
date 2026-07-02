using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exor455.NdmfTween.Editor
{
    /// <summary>
    /// Resolves material property names against a renderer's shared materials, with a
    /// remapping heuristic for Poiyomi's locked-shader property renaming
    /// ("_Prop" -> "_Prop_<suffix>" when "Rename Animated" is used).
    /// </summary>
    internal static class PoiyomiPropertyResolver
    {
        private static readonly string[] ComponentSuffixes = { ".r", ".g", ".b", ".a", ".x", ".y", ".z", ".w" };

        internal enum ResultKind
        {
            Found,
            Remapped,
            NotFound
        }

        internal struct Result
        {
            public ResultKind kind;
            public string resolvedName;
            public string message;
        }

        /// <summary>
        /// Splits an optional vector/color component suffix from a property reference.
        /// "_Prop.x" -> ("_Prop", ".x"); "_Prop" -> ("_Prop", "").
        /// </summary>
        internal static void SplitComponentSuffix(string requested, out string baseName, out string suffix)
        {
            baseName = requested;
            suffix = "";
            foreach (var s in ComponentSuffixes)
            {
                if (requested.EndsWith(s, System.StringComparison.Ordinal))
                {
                    baseName = requested.Substring(0, requested.Length - s.Length);
                    suffix = s;
                    return;
                }
            }
        }

        internal static Result Resolve(Renderer renderer, string requested, bool autoRemap)
        {
            var result = new Result { kind = ResultKind.NotFound, resolvedName = requested, message = null };
            if (renderer == null || string.IsNullOrWhiteSpace(requested)) return result;

            SplitComponentSuffix(requested.Trim(), out var baseName, out var suffix);

            var materials = renderer.sharedMaterials.Where(m => m != null).ToArray();
            if (materials.Length == 0)
            {
                result.message = "対象Rendererにマテリアルが割り当てられていません。";
                return result;
            }

            if (materials.Any(m => m.HasProperty(baseName)))
            {
                result.kind = ResultKind.Found;
                result.resolvedName = baseName + suffix;
                return result;
            }

            // Poiyomi locked-shader rename heuristic: renamed properties keep the original
            // name as a prefix followed by an underscore-separated suffix.
            var candidates = EnumeratePropertyNames(materials)
                .Where(n => n.StartsWith(baseName + "_", System.StringComparison.Ordinal))
                .Distinct()
                .ToList();

            if (autoRemap && candidates.Count == 1)
            {
                result.kind = ResultKind.Remapped;
                result.resolvedName = candidates[0] + suffix;
                result.message = $"プロパティ '{baseName}' は見つかりませんでしたが、'{candidates[0]}' に自動リマップしました(Poiyomiロックによるリネームと推定)。";
                return result;
            }

            result.message = candidates.Count > 0
                ? $"プロパティ '{baseName}' が見つかりません。候補: {string.Join(", ", candidates.Take(5))}"
                : $"プロパティ '{baseName}' が対象マテリアルのどのシェーダーにも存在しません。";
            return result;
        }

        /// <summary>
        /// Best-effort warning for Poiyomi materials that are not yet locked and whose
        /// property lacks the ThryEditor "Animated" tag (animation would break after locking).
        /// Returns null when no warning applies.
        /// </summary>
        internal static string CheckPoiyomiAnimatedTag(Renderer renderer, string requested)
        {
            if (renderer == null || string.IsNullOrWhiteSpace(requested)) return null;
            SplitComponentSuffix(requested.Trim(), out var baseName, out _);

            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat == null || mat.shader == null) continue;
                var shaderName = mat.shader.name;
                if (shaderName.IndexOf("poiyomi", System.StringComparison.OrdinalIgnoreCase) < 0) continue;
                if (shaderName.StartsWith("Hidden/Locked", System.StringComparison.OrdinalIgnoreCase)) continue;
                if (!mat.HasProperty(baseName)) continue;

                var tag = mat.GetTag(baseName.TrimStart('_') + "Animated", false, "");
                if (string.IsNullOrEmpty(tag))
                {
                    tag = mat.GetTag(baseName + "Animated", false, "");
                }

                if (string.IsNullOrEmpty(tag))
                {
                    return $"マテリアル '{mat.name}' はPoiyomi(未ロック)です。'{baseName}' がAnimated指定されていない場合、ロック後にトゥイーンが無効になります。Poiyomiインスペクタでプロパティを右クリック→Animatedを指定してください。";
                }
            }

            return null;
        }

        internal static IEnumerable<string> EnumeratePropertyNames(IEnumerable<Material> materials)
        {
            foreach (var mat in materials)
            {
                if (mat == null || mat.shader == null) continue;
                var shader = mat.shader;
                int count = shader.GetPropertyCount();
                for (int i = 0; i < count; i++)
                {
                    yield return shader.GetPropertyName(i);
                }
            }
        }
    }
}
