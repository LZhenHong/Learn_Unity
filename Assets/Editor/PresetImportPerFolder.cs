using System.IO;
using UnityEditor;
using UnityEditor.Presets;

public class PresetImportPerFolder : AssetPostprocessor
{
    void OnPreprocessAsset()
    {
        if (assetImporter.importSettingsMissing)
        {
            var path = Path.GetDirectoryName(assetPath);
            while (!string.IsNullOrEmpty(path))
            {
                var presetGuides = AssetDatabase.FindAssets("t:Preset", new[]
                {
                    path
                });
                foreach (var presetGuide in presetGuides)
                {
                    string presetPath = AssetDatabase.GUIDToAssetPath(presetGuide);
                    if (Path.GetDirectoryName(presetPath) == path)
                    {
                        var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                        if (preset.ApplyTo(assetImporter))
                        {
                            return;
                        }
                    }
                }
                path = Path.GetDirectoryName(path);
            }
        }
    }
}
