using UnityEngine;
using UnityEditor;

public class StartStopAssetEditingExample: MonoBehaviour
{
    [MenuItem("APIExample/StartStopAssetEditing")]
    static void CallAssetDatabaseAPIsBetweenStartStopAssetEditing()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            AssetDatabase.CopyAsset("Assets/Assets1.txt", "Assets/Text/Assets1.txt");
            AssetDatabase.MoveAsset("Assets/Assets2.txt", "Assets/Text/Assets2.txt");
            AssetDatabase.DeleteAsset("Assets/Assets3.txt");
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }
    }
}
