# AssetBundle

**AssetBundle** 是一个存档文件，包含可在运行时由 Unity 加载的特定于平台的非代码资源（比如模型、纹理、预制件、音频剪辑甚至整个场景）。AssetBundle 可以表示彼此之间的依赖关系；例如，一个 AssetBundle 中的材质可以引用另一个 AssetBundle 中的纹理。为了提高通过网络传输的效率，可以根据用例要求（LZMA 和 LZ4）选用内置算法选择来压缩 AssetBundle。

AssetBundle 可用于可下载内容（DLC），减小初始安装大小，加载针对最终用户平台优化的资源，以及减轻运行时内存压力。

UnityWebRequest 有一个特定的句柄来处理 AssetBundle：`DownloadHandlerAssetBundle`，可根据请求获取 AssetBundle。

下面这些额外提示都有助于掌控全局：

- 将频繁更新的对象与很少更改的对象拆分到不同的 AssetBundle 中。
- 将可能同时加载的对象分到一组。例如模型及其纹理和动画。
- 如果发现多个 AssetBundle 中的多个对象依赖于另一个完全不同的 AssetBundle 中的单个资源，请将依赖项移动到单独的 AssetBundle。如果多个 AssetBundle 引用其他 AssetBundle 中的同一组资源，一种有价值的做法可能是将这些依赖项拉入一个共享 AssetBundle 来减少重复。
- 如果不可能同时加载两组对象（例如标清资源和高清资源），请确保它们位于各自的 AssetBundle 中。
- 如果一个 AssetBundle 中只有不到 50% 的资源经常同时加载，请考虑拆分该捆绑包。
- 考虑将多个小型的（少于 5 到 10 个资源）但经常同时加载内容的 AssetBundle 组合在一起。
- 如果一组对象只是同一对象的不同版本，请考虑使用 AssetBundle 变体。

## 构建 AssetBundle

虽然可以根据需求变化和需求出现而自由组合 `BuildAssetBundleOptions`，但有三个特定的 `BuildAssetBundleOptions` 可以处理 AssetBundle 压缩：

- `BuildAssetBundleOptions.None`：此捆绑包选项使用 LZMA 格式压缩，这是一个压缩的 LZMA 序列化数据文件流。LZMA 压缩要求在使用捆绑包之前对整个捆绑包进行解压缩。此压缩使文件大小尽可能小，但由于需要解压缩，加载时间略长。值得注意的是，在使用此 BuildAssetBundleOptions 时，为了使用捆绑包中的任何资源，必须首先解压缩整个捆绑包。
  解压缩捆绑包后，将使用 LZ4 压缩技术在磁盘上重新压缩捆绑包，这不需要在使用捆绑包中的资源之前解压缩整个捆绑包。最好在包含资源时使用，这样，使用捆绑包中的一个资源意味着将加载所有资源。这种捆绑包的一些用例是打包角色或场景的所有资源。
  由于文件较小，建议仅从异地主机初次下载 AssetBundle 时才使用 LZMA 压缩。通过 [UnityWebRequestAssetBundle](https://docs.unity.cn/cn/2019.4/ScriptReference/Networking.UnityWebRequestAssetBundle.html) 加载的 LZMA 压缩格式 Asset Bundle 会自动重新压缩为 LZ4 压缩格式并缓存在本地文件系统上。如果通过其他方式下载并存储捆绑包，则可以使用 [AssetBundle.RecompressAssetBundleAsync](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.RecompressAssetBundleAsync.html) API 对其进行重新压缩。
- `BuildAssetBundleOptions.UncompressedAssetBundle`：此捆绑包选项采用使数据完全未压缩的方式构建捆绑包。未压缩的缺点是文件下载大小增大。但是，下载后的加载时间会快得多。
- `BuildAssetBundleOptions.ChunkBasedCompression`：此捆绑包选项使用称为 LZ4 的压缩方法，因此压缩文件大小比 LZMA 更大，但不像 LZMA 那样需要解压缩整个包才能使用捆绑包。LZ4 使用基于块的算法，允许按段或“块”加载 AssetBundle。解压缩单个块即可使用包含的资源，即使 AssetBundle 的其他块未解压缩也不影响。

使用 `ChunkBasedCompression` 时的加载时间与未压缩捆绑包大致相当，额外的优势是减小了占用的磁盘大小。

**注意：**使用 LZ4 压缩和未压缩的 AssetBundle 时，[AssetBundle.LoadFromFile](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.LoadFromFile.html) 仅在内存中加载其内容目录，而未加载内容本身。要检查是否发生了此情况，请使用[内存性能分析器 (Memory Profiler)](https://docs.unity.cn/Packages/com.unity.memoryprofiler@0.2/manual/index.html) 包来[检查内存使用情况](https://docs.unity.cn/Packages/com.unity.memoryprofiler@0.2/manual/workflow-memory-usage.html)。

# 本机使用 AssetBundle

可以使用四种不同的 API 来加载 AssetBundle。它们的行为根据加载捆绑包的平台和构建 AssetBundle 时使用的压缩方法（未压缩、LZMA 和 LZ4）而有所不同。

我们必须使用的四个 API 是：

- [AssetBundle.LoadFromMemoryAsync](https://docs.unity.cn/ScriptReference/AssetBundle.LoadFromMemoryAsync.html?_ga=1.226802969.563709772.1479226228)：此函数采用包含 AssetBundle 数据的字节数组。也可以根据需要传递 CRC 值。如果捆绑包采用的是 LZMA 压缩方式，将在加载时解压缩 AssetBundle。LZ4 压缩包则会以压缩状态加载。
- [AssetBundle.LoadFromFile](https://docs.unity.cn/ScriptReference/AssetBundle.LoadFromFile.html?_ga=1.259297550.563709772.1479226228)：从本地存储中加载未压缩的捆绑包时，此 API 非常高效。如果捆绑包未压缩或采用了数据块 (LZ4) 压缩方式，LoadFromFile 将直接从磁盘加载捆绑包。使用此方法加载完全压缩的 (LZMA) 捆绑包将首先解压缩捆绑包，然后再将其加载到内存中。
- [WWW.LoadfromCacheOrDownload](https://docs.unity.cn/ScriptReference/WWW.LoadFromCacheOrDownload.html?_ga=1.226802969.563709772.1479226228)：**即将弃用（使用 UnityWebRequest**）
- [UnityWebRequest](https://docs.unity.cn/ScriptReference/Networking.UnityWebRequest.html?_ga=1.259297550.563709772.1479226228) 的 [DownloadHandlerAssetBundle ](https://docs.unity.cn/ScriptReference/Networking.DownloadHandlerAssetBundle.html?_ga=1.264500235.563709772.1479226228)（Unity 5.3 或更高版本）

`AssetBundle.Unload(true)` 卸载从 AssetBundle 加载的所有游戏对象（及其依赖项）。这不包括复制的游戏对象（例如实例化的游戏对象），因为它们不再属于 AssetBundle。发生这种情况时，从该 AssetBundle 加载的纹理（并且仍然属于它）会从场景中的游戏对象消失，因此 Unity 将它们视为缺少纹理。

通常，使用 `AssetBundle.Unload(false)` 不会带来理想情况。大多数项目应该使用 `AssetBundle.Unload(true)` 来防止在内存中复制对象。

大多数项目应该使用 `AssetBundle.Unload(true)` 并采用一种方法来确保对象不会重复。两种常用方法是：

- 在应用程序生命周期中具有明确定义的卸载瞬态 AssetBundle 的时间点，例如在关卡之间或在加载屏幕期间。

- 维护单个对象的引用计数，仅当未使用所有组成对象时才卸载 AssetBundle。这允许应用程序卸载和重新加载单个对象，而无需复制内存。

如果应用程序必须使用 `AssetBundle.Unload(false)`，则只能以两种方式卸载单个对象：

- 在场景和代码中消除对不需要的对象的所有引用。完成此操作后，调用 [Resources.UnloadUnusedAssets](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.UnloadUnusedAssets.html)。
- 以非附加方式加载场景。这样会销毁当前场景中的所有对象并自动调用 [Resources.UnloadUnusedAssets](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.UnloadUnusedAssets.html)。

# AssetBundle 压缩

## AssetBundle 压缩格式

默认情况下，Unity 通过 LZMA 压缩来创建 AssetBundle，然后通过 LZ4 压缩将其缓存。本部分描述以上的两种压缩格式。

Unity 的 [AssetBundle 构建管线](https://docs.unity.cn/cn/2019.4/Manual/AssetBundles-Building.html)通过 LZMA 压缩来创建 AssetBundle。此压缩格式是表示整个 AssetBundle 的数据流，这意味着如果您需要从这些存档中读取某个资源，就必须将整个流解压缩。这是从内容分发网络 (CDN) 下载的 AssetBundle 的首选格式，因为文件大小小于使用 LZ4 压缩的文件。

另一方面，LZ4 压缩是一种基于块的压缩算法。如果 Unity 需要从 LZ4 存档中访问资源，只需解压缩并读取包含所请求资源的字节的块。这是 Unity 在其两种 AssetBundle 缓存中使用的压缩方法。在构建 AssetBundle 以强制进行 LZ4(HC) 压缩时，应使用 [BuildAssetBundleOptions.ChunkBasedCompression](https://docs.unity.cn/ScriptReference/BuildAssetBundleOptions.ChunkBasedCompression.html) 值。

使用 [BuildAssetBundleOptions.UncompressedAssetBundle](https://docs.unity.cn/ScriptReference/BuildAssetBundleOptions.UncompressedAssetBundle.html) 时由 Unity 构建的未压缩 AssetBundle 无需解压缩，但是会占用更多磁盘空间。

## AssetBundle 缓存

为了使用 WWW 或 [UnityWebRequest](https://docs.unity.cn/ScriptReference/Networking.UnityWebRequest.html) (UWR) 来优化 LZMA AssetBundle 的提取、再压缩和版本控制，Unity 有两种缓存：

- **内存缓存**以 [UncompressedRuntime](https://docs.unity.cn/ScriptReference/BuildCompression.UncompressedRuntime.html) 格式将 AssetBundle 存储在 RAM 中。
- **磁盘缓存**将提取的 AssetBundle 以下文描述的压缩格式存储在可写介质中。

将 AssetBundle 加载到内存缓存中会耗用大量的内存。除非您特别希望频繁且快速地访问 AssetBundle 的内容，否则内存缓存的性价比可能不高。因此，应改用磁盘缓存。

如果向 UWR API 提供版本参数，Unity 会将 AssetBundle 数据存储在磁盘缓存中。如果没有提供版本参数，Unity 将使用内存缓存。版本参数可以是版本号或哈希。如果 [Caching.compressionEnabled](https://docs.unity.cn/ScriptReference/Caching-compressionEnabled.html) 设置为 true，则对于所有后续下载，Unity 会在将 AssetBundle 写入磁盘时应用 LZ4 压缩。它不会压缩缓存中的现有未压缩数据。如果 [Caching.compressionEnabled](https://docs.unity.cn/ScriptReference/Caching-compressionEnabled.html) 设置为 false，Unity 在将 AssetBundle 写入磁盘时不会应用压缩。

最初加载缓存的 LZMA AssetBundle 所花费的时间更长，因为 Unity 必须将存档重新压缩为目标格式。随后的加载将使用缓存版本。

[AssetBundle.LoadFromFile](https://docs.unity.cn/ScriptReference/AssetBundle.LoadFromFile.html) 或 [AssetBundle.LoadFromFileAsync](https://docs.unity.cn/ScriptReference/AssetBundle.LoadFromFileAsync.html) 始终对 LZMA AssetBundle 使用内存缓存，因此您应该使用 UWR API。如果无法使用 UWR API，您可以使用 [AssetBundle.RecompressAssetBundleAsync](https://docs.unity.cn/ScriptReference/AssetBundle.RecompressAssetBundleAsync.html) 将 LZMA AssetBundle 重写到磁盘中。

内部测试表明，使用磁盘缓存而不是内存缓存在 RAM 使用率方面至少存在一个数量级的差异。因此，必须在内存影响、增加的磁盘空间要求以及应用程序的资源实例化时间之间进行权衡。

# 资源重复

当对象构建到 AssetBundle 中时，Unity 5 的 AssetBundle 系统会查找对象的所有依赖项。这是使用资源数据库完成的。此依赖关系信息用于确定包含在 AssetBundle 中的对象集。

显式分配给 AssetBundle 的对象将仅构建到该 AssetBundle 中。当对象的 AssetImporter 将其 assetBundleName 属性设置为非空字符串时，表示“显式指定”该对象。

未显式分配到 AssetBundle 中的任何对象将包含在所有 AssetBundle 中，这些 AssetBundle 会包含一个或多个引用该未标记对象的对象。

如果将两个不同的对象分配给两个不同的 AssetBundle，但两者都引用了一个共同的依赖项对象，那么该依赖项对象将被复制到两个 AssetBundle 中。复制的依赖项也将被实例化，这意味着依赖项对象的两个副本将被视为具有不同标识符的不同对象。这将增加应用程序的 AssetBundle 的总大小。如果应用程序加载对象的两个父项，则还会导致将两个不同的对象副本加载到内存中。

通过组合 AssetDatabase 和 AssetImporter API，可以编写一个 Editor 脚本，确保将所有 AssetBundle 的直接或间接依赖项都分配给 AssetBundle，或者不会有两个 AssetBundle 共享尚未分配给 AssetBundle 的依赖项。由于复制资源的内存成本，建议所有项目都采用这样的脚本。

## 精灵图集重复

以下部分将介绍 Unity 5 的资源依赖性计算代码在与自动生成的精灵图集结合使用时出现的奇怪行为。

任何自动生成的精灵图集都将与生成精灵图集的精灵对象一起分配到同一个 AssetBundle。如果精灵对象被分配给多个 AssetBundle，则精灵图集将不会被分配给 AssetBundle 并且将被复制。如果精灵对象未分配给 AssetBundle，则精灵图集也不会分配给 AssetBundle。

为了确保精灵图集不重复，请确保标记到相同精灵图集的所有精灵都被分配到同一个 AssetBundle。

## Android 纹理

由于 Android 生态系统中存在严重的设备碎片，因此通常需要将纹理压缩为多种不同的格式。虽然所有 Android 设备都支持 ETC1，但 ETC1 不支持具有 Alpha 通道的纹理。如果应用程序不需要 OpenGL ES 2 支持，解决该问题的最简单方法是使用所有 Android OpenGL ES 3 设备都支持的 ETC2。

在运行时，可以使用 [SystemInfo.SupportsTextureFormat](http://docs.unity.cn/ScriptReference/SystemInfo.SupportsTextureFormat.html?_ga=1.141687282.1751468213.1479139860) API 检测对不同纹理压缩格式的支持情况。应使用此信息来选择和加载含有以受支持格式压缩的纹理的 AssetBundle 变体。

# 在运行时加载资源

Unity 支持项目中的__资源文件夹__，允许在主游戏文件中提供内容，但在请求之前不加载这些内容。也可以创建__资源包__。这些文件完全独立于主游戏文件，其中包含游戏按需从文件或 URL 访问的资源。

## 资源包 (Asset Bundle)

资源包是外部资源集合。您可以拥有许多资源包，因此可以拥有许多不同的外部资源集合。这些文件存在于构建的 Unity 播放器之外，通常位于 Web 服务器上，供最终用户动态访问。

要构建资源包，可在 Editor 脚本中调用 [BuildPipeline.BuildAssetBundles()](https://docs.unity.cn/cn/2019.4/ScriptReference/BuildPipeline.BuildAssetBundles.html)。在参数中，指定要包含在构建文件中的__对象__数组以及其他一些选项。这将构建一个文件，稍后即可使用 [AssetBundle.LoadAsset()](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.LoadAsset.html) 在运行时动态加载该文件。

## 资源文件夹 (Resources)

资源文件夹是包含在构建的 Unity 播放器中但不一定链接到 Inspector 中任何游戏对象的资源集合。

要将任何内容放入资源文件夹，只需在 __Project 视图__中创建一个新文件夹，并将该文件夹命名为“Resources”。可在项目中以不同方式组织多个资源文件夹。每当想从其中一个文件夹加载资源时，请调用 [Resources.Load()](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html)。

#### 注意：

在 Resources 文件夹中找到的所有资源及其依赖项都存储在名为 *resources.assets* 的文件中。如果一个资源已被另一个关卡使用，则该资源会存储在该关卡的 *.sharedAssets* 文件中。 **Edit > PlayerSettings** **First Streamed Level** 设置决定了在哪个关卡收集 *resources.assets* 并将其包含在构建中。

通过 [Resources.Load()](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html) 只能访问 _Resources 文件夹_中的资源。然而，更多资源可能最终出现在“resources.assets”文件中，因为它们是依赖项。（例如，Resources 文件夹中的材质可能引用 Resources 文件夹之外的纹理）

## 资源卸载

可通过调用 [AssetBundle.Unload()](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.Unload.html) 来卸载 AssetBundle 的资源。如果为 **unloadAllLoadedObjects** 参数传递 __true__，则 AssetBundle 内部保存的对象和使用 [AssetBundle.LoadAsset()](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.LoadAsset.html) 从 AssetBundle 加载的对象都将被销毁并且捆绑包使用的内存将被释放。

有时可能更希望加载 AssetBundle，实例化所需的对象，并释放捆绑包使用的内存，同时保留对象。好处是可以释放内存来用于其他任务，例如加载另一个 AssetBundle。在这种情况下，可传递 **false** 作为参数。销毁资源包后，无法再加载其中的对象。

如果要在加载另一个关卡之前销毁使用 [Resources.Load()](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html) 加载的场景对象，请对这些对象调用 [Object.Destroy()](https://docs.unity.cn/cn/2019.4/ScriptReference/Object.Destroy.html)。要释放资源，请使用 [Resources.UnloadUnusedAssets()](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.UnloadUnusedAssets.html)。

## 流媒体资源 (Streaming Assets)

Unity 会将放置在 Unity 项目中名为 **StreamingAssets**（区分大小写）的文件夹中的所有文件逐字复制到目标计算机上的特定文件夹。要获取此文件夹，请使用 [Application.streamingAssetsPath](https://docs.unity.cn/cn/2019.4/ScriptReference/Application-streamingAssetsPath.html) 属性。在任何情况下，最好使用 `Application.streamingAssetsPath` 来获取 **StreamingAssets** 文件夹的位置，因为它总是指向运行应用程序的平台上的正确位置。

`Application.streamingAssetsPath` 返回的位置因平台而异：

- 大多数平台（Unity Editor、Windows、Linux 播放器、PS4、Xbox One、Switch）使用 `Application.dataPath + "/StreamingAssets"`。
- macOS 播放器使用 `Application.dataPath + "/Resources/Data/StreamingAssets"`。
- iOS 使用 `Application.dataPath + "/Raw"`。
- Android 使用经过压缩的 APK/JAR 文件中的文件：`"jar:file://" + Application.dataPath + "!/assets"`。

在许多平台上，流媒体资源文件夹位置是只读的；您不能在运行时在这些位置修改或写入新文件。请使用 [Application.persistentDataPath](https://docs.unity.cn/cn/2019.4/ScriptReference/Application-persistentDataPath.html) 来获取可写的文件夹位置。

**注意**：位于 **StreamingAssets** 文件夹中的 .dll 和脚本文件不参与脚本编译。