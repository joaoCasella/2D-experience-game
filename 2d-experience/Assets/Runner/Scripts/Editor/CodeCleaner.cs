using UnityEditor;

namespace Runner.Scripts.Editor
{
    public class CodeCleaner
    {
        [MenuItem("Custom/Code cleaner/Force reserialize assets", false, 20)]
        public static void ForceReserializeAssets()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
