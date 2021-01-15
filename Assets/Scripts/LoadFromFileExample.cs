using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;

public class LoadFromFileExample : MonoBehaviour
{
    IEnumerator InstantiateObject()
    {
        string url = "file:///" + Application.dataPath + "/AssetBundles/testassetbundle";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject cube = bundle.LoadAsset<GameObject>("Cube");
        Instantiate(cube);
        GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        Instantiate(sprite);
    }

    IEnumerator LoadFromMemoryAsync(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
        yield return request;
        AssetBundle bunlde = request.assetBundle;
        var prefab = bunlde.LoadAsset<GameObject>("MyObject");
        Instantiate(prefab);
    }

    public void LoadAssetBundleFromFile()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myAssetBundle"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load asset bundle from file.");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
        Instantiate(prefab);
    }

    public IEnumerator LoadAssetAsync()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myAssetBundle"));
        AssetBundleRequest request = myLoadedAssetBundle.LoadAssetAsync<GameObject>("myAssetBundle");
        yield return request;
        var loadedAsset = request.asset;
        Instantiate(loadedAsset);
    }

    void Start()
    {
        // 它所描述的是在构建 AssetBundles 时所生成的清单列表，使用构建函数在指定目录下构建 AssetBundles 时会生成一个总的 Manifest 文件，它的资源文件名与其所在的文件夹名称相同。
        LoadAssetBundleManifest(Path.Combine(Application.dataPath, "AssetBundles/AssetBundles"));
    }

    public void LoadAssetBundleManifest(string path)
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        Debug.Log($"Path: {path}");
        if (assetBundle == null)
        {
            Debug.Log("Failed to load asset bundle from file.");
            return;
        }
        AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] dependencies = manifest.GetAllDependencies("assets"); // 传递想要依赖项的捆绑包的名称
        foreach (string dependency in dependencies)
        {
            AssetBundle.LoadFromFile(Path.Combine(path, dependency));
        }
    }
}
