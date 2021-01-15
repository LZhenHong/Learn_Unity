using UnityEngine;
using UnityEditor;

public class AssetDatabaseIOExample : MonoBehaviour
{
    [MenuItem("AssetDatabase/FileOperationsExample")]
    static void Example()
    {
        string ret;
        Material material = new Material(Shader.Find("Specular"));
        AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");
        if (AssetDatabase.Contains(material))
        {
            Debug.Log("Material asset created.");
        }
        
        ret = AssetDatabase.RenameAsset("Assets/MyMaterial.mat", "MyMaterialNew");
        if (ret == "")
        {
            Debug.Log("Material asset renamed to MyMaterialNew");
        }
        else
        {
            Debug.Log(ret);
        }

        ret = AssetDatabase.CreateFolder("Assets", "NewFolder");
        if (AssetDatabase.GUIDToAssetPath(ret) != "")
        {
            Debug.Log("Folder asset created.");
        }
        else
        {
            Debug.Log("Couldn't find GUID for the path.");
        }

        ret = AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(material), "Assets/NewFolder/MyMaterialNew.mat");
        if (ret == "")
        {
            Debug.Log("Material asset moved to NewFolder/MyMaterialNew.mat");
        }
        else
        {
            Debug.Log(ret);
        }

        if (AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(material), "Assets/MyMaterialNew.mat"))
        {
            Debug.Log("Material asset copied as Assets/MyMaterialNew.mat");
        }
        else
        {
            Debug.Log("Couldn't copy the material");
        }
        AssetDatabase.Refresh();

        Material copiedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/MyMaterialNew.mat");
        if (AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(copiedMaterial)))
        {
            Debug.Log("Material copy move to trash.");
        }

        if (AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(material)))
        {
            Debug.Log("Material asset deleted");
        }
        if (AssetDatabase.DeleteAsset("Assets/NewFolder"))
        {
            Debug.Log("NewFolder deleted.");
        }

        AssetDatabase.Refresh();
    }
}
