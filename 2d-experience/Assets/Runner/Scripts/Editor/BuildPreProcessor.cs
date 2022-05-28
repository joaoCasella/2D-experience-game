#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Runner.Scripts.Editor
{
    public class BuildPreProcessor
    {
        // (24/05/2022) Based on solution available at https://gist.github.com/favoyang/cd2cf2ed9df7e2538f3630610c604c51
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
        }

        private static void BuildPlayerHandler(BuildPlayerOptions options)
        {
            if (EditorUtility.DisplayDialog("Build with Addressables",
                "Do you want to build a clean addressables before export?",
                "Build with Addressables", "Skip"))
            {
                PreExport();
            }

            BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
        }

        /// <summary>
        /// Run a clean build before export.
        /// </summary>
        private static void PreExport()
        {
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();
        }
    }
}

#endif