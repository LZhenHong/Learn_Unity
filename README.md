# Learn Unity

1. Unity 资源工作流程
2. Unity 与原生层的交互
3. Prefab 的使用
4. Unity Package \ Import Setting
5. DLL 的热更以及 DLL 相互之间的依赖
6. Unity UGUI 的基本使用
7. Unity GameObject 与脚本之间的关系
8. Unity 相机的使用
9. Unity AssetBundle 之间的相互依赖，以及打包时如何将资源生成 AssetBundle
10. ILRuntime
11. Unity 打包的命令及内部详细过程
12. 对 Unity Editor 的扩展，以及与项目开发的结合
13. Unity 3D 项目的知识
14. C# IEnumerator 与 yield return
15. Unity 协程

## Unity 与 VS

1. 尽管 Visual Studio 附带了自己的 C# 编译器，并且您可以使用它来检查 # 脚本中是否存在错误，但 Unity 仍然使用自己的 C# 编译器来编译脚本。使用 Visual Studio 编译器仍然非常有用，因为这意味着不必一直切换到 Unity 来检查是否有任何错误。
2. Visual Studio 的 C# 编译器比 Unity 的 C# 编译器目前支持的功能更多。也就是说，某些代码（尤其是较新的 # 功能）不会在 Visual Studio 中抛出错误，但在 Unity 中则会。
3. **Unity 会自动创建和维护 Visual Studio .sln 和 .csproj 文件。每当在 Unity 中添加/重命名/移动/删除文件时，Unity 都会重新生成 .sln 和 .csproj 文件。**也可以从 Visual Studio 向解决方案添加文件。Unity 随后会导入这些新文件，下次 Unity 再次创建项目文件时，便会使用包含的新文件进行创建。

## Unity 特殊文件夹名称

### Assets

**Assets** 文件夹是包含 Unity 项目使用的资源的主文件夹。Editor 中的 Project 窗口的内容直接对应于 Assets 文件夹的内容。大多数 API 函数都假定所有内容都位于 Assets 文件夹中，因此不要求显式提及该文件夹。但是，有些函数需要将 Assets 文件夹作为路径名的一部分添加（例如，[AssetDatabase](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.html) 类中的一些函数）。

### Editor

放在名为 **Editor** 的文件夹中的脚本被视为 Editor 脚本而不是运行时脚本。这些脚本在开发期间向 Editor 添加功能，并在运行时在构建中不可用。

可在 Assets 文件夹中的任何位置添加多个 Editor 文件夹。应将 Editor 脚本放在 Editor 文件夹内或其中的子文件夹内。

Editor 文件夹的具体位置会影响其脚本相对于其他脚本的编译时间（有关此方面的完整说明，请参阅[特殊文件夹和脚本编译顺序](https://docs.unity.cn/cn/2019.4/Manual/ScriptCompileOrderFolders.html)的相关文档）。使用 Editor 脚本中的 [EditorGUIUtility.Load](https://docs.unity.cn/cn/2019.4/ScriptReference/EditorGUIUtility.Load.html) 函数可从 Editor 文件夹中的 Resources 文件夹加载资源。这些资源只能通过 Editor 脚本加载，并会从构建中剥离。

**注意：**如果脚本位于 Editor 文件夹中，Unity 不允许将派生自 MonoBehaviour 的组件分配给游戏对象。

### Gizmos

[Gizmos](https://docs.unity.cn/cn/2019.4/ScriptReference/Gizmos.html) 允许将图形添加到 Scene 视图，以帮助可视化不可见的设计细节。[Gizmos.DrawIcon](https://docs.unity.cn/cn/2019.4/ScriptReference/Gizmos.DrawIcon.html) 函数在场景中放置一个图标，作为特殊对象或位置的标记。必须将用于绘制此图标的图像文件放在名为 **Gizmos** 的文件夹中，这样才能被 DrawIcon 函数找到。

只能有一个 Gizmos 文件夹，且必须将其放在项目的根目录；直接位于 Assets 文件夹中。将所需的资源文件放在此 Gizmos 文件夹内或其中的子文件夹内。如果资源文件位于子文件夹中，请始终在传递给 [Gizmos.DrawIcon](https://docs.unity.cn/cn/2019.4/ScriptReference/Gizmos.DrawIcon.html) 函数的路径中包含子文件夹路径。

### Resources

可从脚本中按需加载资源，而不必在场景中创建资源实例以用于游戏。为此，应将资源放在一个名为 **Resources** 的文件夹中。通过使用 [Resources.Load](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html) 函数即可加载这些资源。

可在 Assets 文件夹中的任何位置添加多个 Resources 文件夹。将所需的资源文件放在 Resources 文件夹内或其中的子文件夹内。如果资源文件位于子文件夹中，请始终在传递给 [Resources.Load](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html) 函数的路径中包含子文件夹路径。

请注意，如果 Resources 文件夹是 Editor 的子文件夹，则其中的资源可通过 Editor 脚本加载，但会从构建中剥离。

### StreamingAssets

尽管将资源直接合并到构建中更为常见，但有时可能希望资源以其原始格式作为单独的文件提供。例如，需要从文件系统访问视频文件，而不是用作 MovieTexture 在 iOS 上播放该视频。将一个文件放在名为 **StreamingAssets** 的文件夹中，这样就会将其按原样复制到目标计算机，然后就能从特定文件夹中访问该文件。请参阅关于[流媒体资源 (Streaming Assets)](https://docs.unity.cn/cn/2019.4/Manual/StreamingAssets.html) 的页面以了解更多详细信息。

只能有一个 StreamingAssets 文件夹，且必须将其放在项目的根目录；直接位于 Assets 文件夹中。将所需的资源文件放在此 StreamingAssets 文件夹内或其中的子文件夹内。如果资源文件位于子文件夹中，请始终在用于引用流媒体资源的路径中包含子文件夹路径。

## 2D 图形

2D 图形对象称为__精灵__。精灵本质上只是标准纹理，但可通过一些特殊技巧在开发过程中组合和管理精灵纹理以提高效率和方便性。Unity 提供内置的 [Sprite Editor](https://docs.unity.cn/cn/2019.4/Manual/SpriteEditor.html)，允许从更大图像提取精灵图形。因此可以在图像编辑器中编辑单个纹理内的多个组件图像。例如，可以使用此工具将角色的手臂、腿和身体保持为一个图像中的单独元素。

应使用 [Sprite Renderer](https://docs.unity.cn/cn/2019.4/Manual/class-SpriteRenderer.html) 组件而不是用于 3D 对象的 [Mesh Renderer](https://docs.unity.cn/cn/2019.4/Manual/class-MeshRenderer.html) 来渲染精灵。可通过 Components 菜单 (**Component > Rendering > Sprite Renderer**) 将精灵渲染器 (Sprite Renderer) 添加到游戏对象，也可直接创建已附加精灵渲染器的游戏对象（菜单：__GameObject > 2D Object > Sprite__）。

此外，可以使用 [Sprite Creator](https://docs.unity.cn/cn/2019.4/Manual/SpriteCreator.html) 工具来创建 2D 占位图像。

## 2D 排序

### 排序图层和图层中的顺序

可以通过 Inspector 窗口或通过 Unity Scripting API 将[排序图层 (Sorting Layer)](https://docs.unity.cn/Manual/class-TagManager.html#SortingLayers) 和**图层中的顺序 (Order in Layer)**（位于渲染器的 **Property** 设置中）用于所有 2D 渲染器。为确定渲染器在渲染队列中的优先级，可以将渲染器设置为现有的**排序图层**，或创建一个新排序图层。更改 **Order in Layer** 的值，即可设置渲染器在同一**排序图层**中的其他渲染器之间的优先级。

## 排序组

### 对排序组中的渲染器进行排序

Unity 按 **Sorting Layer** 和 **Order in Layer** [渲染器属性](https://docs.unity.cn/cn/2019.4/Manual/class-SpriteRenderer.html)对同一排序组中的所有渲染器进行排序。在此排序过程中，Unity 不会考虑每个渲染器的 **Distance to Camera** 属性。实际上，Unity 会根据包含 Sorting Group 组件的根游戏对象的位置，为整个排序组（包括其所有子渲染器）设置 Distance to Camera 值。

## 精灵图集

2D 项目使用精灵和其他图形来创建其场景的视觉效果。这意味着单个项目可能包含许多纹理文件。Unity 通常会为场景中的每个纹理发出一个[绘制调用](https://docs.unity.cn/cn/2019.4/Manual/DrawCallBatching.html)；但是，在具有许多纹理的项目中，多个绘制调用会占用大量资源，并会对项目的性能产生负面影响。

**Allow Rotation**：选中此复选框允许在 Unity 将精灵打包到精灵图集时旋转精灵。这样可以最大限度提高组合后的纹理中的精灵密度，并且默认情况下会启用此选项。如果精灵图集包含[画布 UI](https://docs.unity.cn/cn/2019.4/Manual/UICanvas.html) 元素纹理，请禁用此选项，因为 Unity 在打包期间旋转精灵图集中的纹理时，也会在场景中旋转它们的方向。

当精灵在场景中处于活动状态时，Unity 会加载该精灵所属的精灵图集以及该精灵包含的所有纹理。如果 Unity 加载具有巨大纹理的精灵图集，而场景中没有任何对象使用大多数的这些纹理时，这样做会导致过高的性能开销。

## 图形

### 渲染管线

不同的渲染管线具有不同的功能和性能特征，并且适用于不同的游戏、应用程序和平台。将项目从一个渲染管线切换到另一个渲染管线可能很困难，因为不同的渲染管线使用不同的着色器输出，并且可能没有相同的特性。因此，必须要了解 Unity 提供的不同渲染管线，以便可以在开发早期为项目做出正确决定。

### 延迟着色渲染路径

使用延迟着色时，可影响游戏对象的光源数量没有限制。所有光源都按像素进行评估，这意味着它们都能与法线贴图等正确交互。此外，所有光源都可以有剪影和阴影。

延迟着色的优点是，光照的处理开销与接受光照的像素数成正比。这取决于场景中的光量大小，而不管接受光照的游戏对象有多少。因此，可通过减少光源数量来提高性能。延迟着色还具有高度一致和可预测的行为。每个光源的效果都是按像素计算的，因此不会有在大三角形上分解的光照计算。

在缺点方面，延迟着色并不支持抗锯齿，也无法处理半透明游戏对象（这些对象使用[前向](https://docs.unity.cn/cn/2019.4/Manual/RenderTech-ForwardRendering.html)渲染进行渲染）。

**注意：**使用正交投影 (Orthographic projection) 时不支持延迟渲染。如果摄像机的投影模式设置为正交模式，则摄像机将回退到前向渲染。

## 常见资源类型

### FBX 和模型文件

由于 Unity 支持 FBX 文件格式，因此可以从任何支持 FBX 的 3D 建模软件导入数据。Unity 也支持本机导入 SketchUp 文件。如需了解如何在从 3D 建模软件导出 FBX 文件时获得最佳结果，请参阅[优化 FBX 文件](https://docs.unity.cn/cn/2019.4/Manual/HOWTO-importObject.html)。

**注意：**还可以使用原生格式（例如 .max、.blend、.mb 和 .ma）从最常见的 3D 建模软件中保存 3D 文件。Unity 在 `Assets` 文件夹中找到这些文件时，会通过回调 3D 建模软件的 FBX 导出插件来导入它们。但是，建议将它们导出为 FBX 格式。

## 资源数据库

对于大多数类型的资源，Unity 需要将资源的源文件中的数据转换为可用于游戏或实时应用程序的格式。这些转换后的文件及其关联的数据会存储在**资源数据库 (Asset Database)** 中。

由于大多数文件格式都经过优化来节省存储空间，所以需要执行转换过程，而在游戏或实时应用程序中，资源数据需要采用可用于硬件（例如 CPU、显卡或音频硬件）的格式才能立即使用。例如，Unity 将 .png 图像文件导入为纹理时，在运行时不会使用原始的 .png 格式数据。而在导入纹理时，Unity 将以另一种格式创建图像的新表示形式，并将其存储在项目的 *Library* 文件夹中。Unity 引擎中的 Texture 类会使用此导入版本，然后 Unity 将其上传到 GPU 以进行实时显示。

### 资源缓存

资源缓存是 Unity 存储导入版本的资源的位置。由于 Unity 始终可以从源资源文件及其依赖项重新创建这些导入的版本，所以这些导入的版本被视为预先计算的数据的缓存。在使用 Unity 时，此缓存可节省时间。因此，应该从版本控制系统中排除资源缓存中的文件。

默认情况下，Unity 使用本地缓存，这意味着导入版本的资源将缓存在本地计算机上项目文件夹的 *Library* 文件夹中。应该使用 **ignore file** 从版本控制中排除此文件夹。

### 源资源和 Artifact

Unity 在 Library 文件夹中保留两个数据库文件，它们统称为资源数据库。这两个数据库可以跟踪有关源资源文件和 Artifact（这是有关导入结果的信息）的信息。

#### 源资源数据库

源资源数据库包含有关源资源文件的元数据信息，Unity 将这些信息用来确定文件是否被修改，从而决定是否应该重新导入文件。这些信息中包括诸如上次修改日期、文件内容哈希、GUID 和其他元数据信息之类的信息。

#### Artifact 数据库

Artifact 是导入过程的结果。Artifact 数据库包含有关每个源资源的导入结果的信息。每个 Artifact 都包含导入依赖项信息、Artifact 元数据信息和 Artifact 文件列表。

**注意：**数据库文件位于项目的 *Library* 文件夹中，因此应从版本控制系统中将这些文件排除。可以在以下位置找到它们：

- 源资源数据库：`Library\SourceAssetDB`
- Artifact 数据库：`Library\ArtifactDB`

### 加载资源

编辑器仅在需要时加载资源，例如，是否将资源添加到场景或从 Inspector 面板中进行编辑。但是，可以使用 [AssetDatabase.LoadAssetAtPath](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.LoadAssetAtPath.html)、[AssetDatabase.LoadMainAssetAtPath](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.LoadMainAssetAtPath.html)、[AssetDatabase.LoadAllAssetRepresentationsAtPath](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.LoadAllAssetRepresentationsAtPath.html) 和 [AssetDatabase.LoadAllAssetsAtPath](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.LoadAllAssetsAtPath.html) 从脚本加载和访问资源。

### 使用 AssetDatabase 进行文件操作

由于 Unity 保留有关资源文件的元数据，因此不应使用文件系统创建、移动或删除这些文件。相反，可以使用 [AssetDatabase.Contains](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.Contains.html)、[AssetDatabase.CreateAsset](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.CreateAsset.html)、[AssetDatabase.CreateFolder](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.CreateFolder.html)、[AssetDatabase.RenameAsset](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.RenameAsset.html)、[AssetDatabase.CopyAsset](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.CopyAsset.html)、[AssetDatabase.MoveAsset](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.MoveAsset.html)、[AssetDatabase.MoveAssetToTrash](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.MoveAssetToTrash.html) 和 [AssetDatabase.DeleteAsset](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.DeleteAsset.html)。

## 刷新资源数据库

Unity 在资源数据库刷新期间执行以下步骤：

1. 查找资源文件的更改，然后更新源资源数据库。
2. 导入并编译与代码相关的文件，例如 .dll、.asmdef、.asmref、.rsp 和 .cs 文件。
3. 然后，如果未从脚本调用 [Refresh](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.Refresh.html)，重新加载该域。 
4. 对导入的代码相关文件的所有资源进行后期处理。
5. 然后导入与代码无关的资源，并对所有剩余的导入资源进行后期处理。
6. 随后对资源进行[热重载](https://docs.unity.cn/cn/2019.4/Manual/AssetDatabaseRefreshing.html#hotreloading)。

### 资源数据库详细刷新过程

Unity 在资源数据库刷新期间执行以上部分中描述的步骤。本部分将进一步详细描述这一过程。这些步骤在一个循环中执行，某些步骤可能导致刷新过程重新启动（例如，导入资源时会创建其他资源，Unity 也需要导入这些资源）。

Unity 在以下情况下会重新启动资源数据库刷新循环：

- 如果导入资源后，导入器用过的文件在磁盘中发生改变。
- 如果在 OnPostProcessAllAssets 中调用了以下任一项：
  - [ForceReserializeAssets](https://docs.unity.cn/ScriptReference/AssetDatabase.ForceReserializeAssets.html)
  - [AssetImporter.SaveAndImport](https://docs.unity.cn/ScriptReference/AssetImporter.SaveAndReimport.html)
  - 导致出现额外“刷新”排队情况的任何 AssetDatabase API，例如 MoveAsset、CreateAsset 和 ImportAsset
- 如果所导入文件的时间戳在导入时发生改变，会使该文件排队等待重新导入
- 导入器在导入过程中创建了文件（例如，FBX 模型可以通过从模型提取纹理来重新启动“刷新”）。

#### 查找磁盘中的更改

Unity 查找磁盘中的更改时会扫描项目中的 ***Assets*** 和 ***Packages*** 文件夹，从而检查自上次扫描以来是否已添加、修改或删除任何文件。它将所有更改收集到列表中，以供下一步处理。

#### 更新源资源数据库

Unity 收集文件列表后，将为添加或修改的文件获取文件哈希。然后会使用这些文件的 GUID 来更新资源数据库，并会删除被检测到“已删除”的文件的条目。

#### 导入和编译代码相关文件

在一系列已更改或已添加的文件中，Unity 收集与代码相关的文件，并将其发送到脚本编译管线。编译器从项目中的脚本文件和程序集定义文件生成程序集。有关此步骤的更多信息，请参阅[脚本编译程序集定义文件](https://docs.unity.cn/Manual/ScriptCompilationAssemblyDefinitionFiles.html)的相关文档。

#### 重新加载域

如果 Unity 检测到任何脚本更改，则会重新加载 C# 域。这样做的原因是可能已创建新的脚本化导入器 (Scripted Importer)，它们的逻辑可能会影响“刷新”队列中的资源导入结果。此步骤会重新启动 Refresh() 以确保所有新的脚本化导入器生效。

#### 导入与代码无关的资源

Unity 导入所有与代码相关的资源并重新加载域后，将继续导入其余资源。每个资源的导入器都会处理该类型的资源，并根据文件扩展名来识别应导入的文件类型。例如，TextureImporter 负责导入 .jpg、.png 和 .psd 文件等。

导入器分为两组：**原生导入器 (Native Importer)** 和**脚本化导入器 (Scripted Importer)**。

原生导入器

原生导入器内置于 Unity 中，并为 Unity 的大多数基本资源类型（例如 3D 模型、纹理和音频文件）提供导入功能。

|               导入器                |                           文件格式                           |
| :---------------------------------: | :----------------------------------------------------------: |
|     AssemblyDefinitionImporter      |                            asmdef                            |
| AssemblyDefinitionReferenceImporter |                            asmref                            |
|            AudioImporter            |       ogg、aif、aiff、flac、wav、mp3、mod、it、s3m、xm       |
|        ComputeShaderImporter        |                           compute                            |
|           DefaultImporter           |                          rsp、unity                          |
|             FBXImporter             |    fbx、mb、ma、max、jas、dae、dxf、obj、c4d、blend、lxo     |
|       IHVImageFormatImporter        |                     astc、dds、ktx、pvr                      |
|        LocalizationImporter         |                              po                              |
|           Mesh3DSImporter           |                             3ds                              |
|        NativeFormatImporter         | anim、animset、asset、blendtree、buildreport、colors、controller、cubemap、curves、curvesNormalized、flare、fontsettings、giparams、gradients、guiskin、ht、mask、mat、mesh、mixer、overrideController、particleCurves、particleCurvesSigned、particleDoubleCurves、particleDoubleCurvesSigned、physicMaterial、physicsMaterial2D、playable、preset、renderTexture、shadervariants、spriteatlas、state、statemachine、texture2D、transition、webCamTexture、brush、terrainlayer、signal |
|       PackageManifestImporter       |                             json                             |
|           PluginImporter            | dll、winmd、so、jar、java、kt、aar、suprx、prx、rpl、cpp、cc、c、h、jslib、jspre、bc、a、m、mm、swift、xib、bundle、dylib、config |
|           PrefabImporter            |                            prefab                            |
|      RayTracingShaderImporter       |                           raytrace                           |
|           ShaderImporter            |               cginc、cg、glslinc、hlsl、shader               |
|          SketchUpImporter           |                             skp                              |
|          SpeedTreeImporter          |                           spm、st                            |
|          SubstanceImporter          |                            .sbsar                            |
|         TextScriptImporter          | txt、html、htm、xml、json、csv、yaml、bytes、fnt、manifest、md、js、boo、rsp |
|           TextureImporter           | jpg、jpeg、tif、tiff、tga、gif、png、psd、bmp、iff、pict、pic、pct、exr、hdr |
|        TrueTypeFontImporter         |                     ttf、dfont、otf、ttc                     |
|          VideoClipImporter          | avi、asf、wmv、mov、dv、mp4、m4v、mpg、mpeg、ogv、vp8、webm  |
|        VisualEffectImporter         |                  vfx、vfxoperator、vfxblock                  |


导入器导入资源文件时，会生成 **AssetImportContext**。AssetImportContext 用于报告资源的静态依赖项。

导入器分为预处理资源导入器和后处理资源导入器。所有导入完成后触发的最后一个后处理回调是 [`OnPostprocessAllAssets`](https://docs.unity.cn/ScriptReference/AssetPostprocessor.OnPostprocessAllAssets.html)。

如果将资源另存为“仅文本”，但是必须将此资源序列化为二进制文件，这时会发生重新启动。

#### 热重载

热重载是指在 Editor 开启的状态下由 Unity 导入脚本和资源并应用所有更改的过程。无论 Editor 是否处于运行模式 (Play Mode)，都可能发生此情况。无需重新启动应用程序和 Editor 即可使更改生效。

更改并保存脚本时，Unity 会热重载所有当前加载的脚本数据。它首先将所有可序列化变量存储在所有加载的脚本中，并在加载脚本后恢复这些变量。热重载后，所有不可序列化的数据都会丢失。

**注意：**默认资源先于脚本资源导入，因此不会为默认资源调用任何由脚本定义的 PostProcessAllAssets 回调。

#### 刷新结束

一旦完成所有这些步骤，就完成了 Refresh() 并使用相关信息更新了 **Artifact 数据库**，还会在磁盘中生成必要的导入结果文件。

## AssetDatabase 批处理

使用批处理可以减少在代码中更改资源时所需的时间和处理量。如果在代码中对多个资源进行更改（例如，复制或移动资源文件），则资源数据库 (Asset Database) 的默认行为是依次处理每个更改，并对资源执行完整的“刷新”过程，然后再转到下一行代码。

### 处理操作的方法

要指定资源数据库应该一次性处理一组操作，可以使用以下方法：[AssetDatabase.StartAssetEditing](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.StartAssetEditing.html) 和 [AssetDatabase.StopAssetEditing](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.StopAssetEditing.html)。

**AssetDatabase.StartAssetEditing**

此方法告诉资源数据库您正在开始对资源进行编辑。资源数据库进入暂停状态，并在您调用相应的 **StopAssetEditing** 方法告诉您已完成之前，不会对资源进行任何进一步的更改。

**AssetDatabase.StopAssetEditing**

完成所有的资源更改后，请调用此方法告诉资源数据库处理您的更改并恢复其立即自动处理更改的正常行为。然后，资源数据库以批处理方式处理您在 `StartAssetEditing` 和 `StopAssetEditing` 之间进行的更改，这种处理方法比逐个处理的速度要快。

#### 嵌套调用 StartAssetEditing 和 StopAssetEditing

如果您对 `StartAssetEditing` 进行了多次调用，必须对 `StopAssetEditing` 进行相应次数的调用，使资源数据库恢复其自动处理更改的正常行为。

这是因为这些函数会递增和递减计数器，而不是充当简单的开/关功能。调用 `StartAssetEditing` 将递增计数器，而调用 `StopAssetEditing` 将递减计数器。当计数器达到零时，资源数据库将恢复其正常行为。

Unity 使用计数器而不是简单开/关布尔值的原因是，通过使用计数器，如果您的代码执行多个嵌套的“start”和“stop”对，内部对不会意外地过早恢复资源数据库的正常行为。相反，每对会将计数器加一或减一，并且如果您的代码正确嵌套，则对 `StopAssetEditing` 的最终外部调用会将计数器设置为零。

**注意：**您的代码绝不应该使计数器降到零以下。这样做会产生错误。

### 使用 try…finally 进行资源编辑

调用 `AssetDatabase.StartAssetEditing` 时，Unity 会将整个 Editor 的 AssetDatabase 置于暂停状态。因此，如果您没有对 AssetDatabase.`StopAssetEditing` 进行相应的调用，则在涉及任何与资源相关的操作（导入、刷新等）时，Editor 会看起来无响应，并需要重新启动 Editor 以恢复其正常运行状态。

不使用 `try` …`finally` 代码块时，如果任何用于修改资源的代码导致了错误，则可能会阻止调用 `StopAssetEditing`。为了避免这种情况，请将这些调用与 `StartAssetEditing` 一起包裹在一个 `try`…`finally` 代码块中，然后将资源修改代码包含在 `try` 代码块中，而将 `StopAssetEditing` 调用放在 `finally` 代码块中。这样可以确保，如果在 `try` 代码块中进行更改时发生了任何异常，仍可以保证将会调用 `AssetDatabase.StopAssetEditing`。

## AssetBundle

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

### 构建 AssetBundle

虽然可以根据需求变化和需求出现而自由组合 `BuildAssetBundleOptions`，但有三个特定的 `BuildAssetBundleOptions` 可以处理 AssetBundle 压缩：

- `BuildAssetBundleOptions.None`：此捆绑包选项使用 LZMA 格式压缩，这是一个压缩的 LZMA 序列化数据文件流。LZMA 压缩要求在使用捆绑包之前对整个捆绑包进行解压缩。此压缩使文件大小尽可能小，但由于需要解压缩，加载时间略长。值得注意的是，在使用此 BuildAssetBundleOptions 时，为了使用捆绑包中的任何资源，必须首先解压缩整个捆绑包。
  解压缩捆绑包后，将使用 LZ4 压缩技术在磁盘上重新压缩捆绑包，这不需要在使用捆绑包中的资源之前解压缩整个捆绑包。最好在包含资源时使用，这样，使用捆绑包中的一个资源意味着将加载所有资源。这种捆绑包的一些用例是打包角色或场景的所有资源。
  由于文件较小，建议仅从异地主机初次下载 AssetBundle 时才使用 LZMA 压缩。通过 [UnityWebRequestAssetBundle](https://docs.unity.cn/cn/2019.4/ScriptReference/Networking.UnityWebRequestAssetBundle.html) 加载的 LZMA 压缩格式 Asset Bundle 会自动重新压缩为 LZ4 压缩格式并缓存在本地文件系统上。如果通过其他方式下载并存储捆绑包，则可以使用 [AssetBundle.RecompressAssetBundleAsync](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.RecompressAssetBundleAsync.html) API 对其进行重新压缩。
- `BuildAssetBundleOptions.UncompressedAssetBundle`：此捆绑包选项采用使数据完全未压缩的方式构建捆绑包。未压缩的缺点是文件下载大小增大。但是，下载后的加载时间会快得多。
- `BuildAssetBundleOptions.ChunkBasedCompression`：此捆绑包选项使用称为 LZ4 的压缩方法，因此压缩文件大小比 LZMA 更大，但不像 LZMA 那样需要解压缩整个包才能使用捆绑包。LZ4 使用基于块的算法，允许按段或“块”加载 AssetBundle。解压缩单个块即可使用包含的资源，即使 AssetBundle 的其他块未解压缩也不影响。

使用 `ChunkBasedCompression` 时的加载时间与未压缩捆绑包大致相当，额外的优势是减小了占用的磁盘大小。

**注意：**使用 LZ4 压缩和未压缩的 AssetBundle 时，[AssetBundle.LoadFromFile](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.LoadFromFile.html) 仅在内存中加载其内容目录，而未加载内容本身。要检查是否发生了此情况，请使用[内存性能分析器 (Memory Profiler)](https://docs.unity.cn/Packages/com.unity.memoryprofiler@0.2/manual/index.html) 包来[检查内存使用情况](https://docs.unity.cn/Packages/com.unity.memoryprofiler@0.2/manual/workflow-memory-usage.html)。

## 本机使用 AssetBundle

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

## AssetBundle 压缩

### AssetBundle 压缩格式

默认情况下，Unity 通过 LZMA 压缩来创建 AssetBundle，然后通过 LZ4 压缩将其缓存。本部分描述以上的两种压缩格式。

Unity 的 [AssetBundle 构建管线](https://docs.unity.cn/cn/2019.4/Manual/AssetBundles-Building.html)通过 LZMA 压缩来创建 AssetBundle。此压缩格式是表示整个 AssetBundle 的数据流，这意味着如果您需要从这些存档中读取某个资源，就必须将整个流解压缩。这是从内容分发网络 (CDN) 下载的 AssetBundle 的首选格式，因为文件大小小于使用 LZ4 压缩的文件。

另一方面，LZ4 压缩是一种基于块的压缩算法。如果 Unity 需要从 LZ4 存档中访问资源，只需解压缩并读取包含所请求资源的字节的块。这是 Unity 在其两种 AssetBundle 缓存中使用的压缩方法。在构建 AssetBundle 以强制进行 LZ4(HC) 压缩时，应使用 [BuildAssetBundleOptions.ChunkBasedCompression](https://docs.unity.cn/ScriptReference/BuildAssetBundleOptions.ChunkBasedCompression.html) 值。

使用 [BuildAssetBundleOptions.UncompressedAssetBundle](https://docs.unity.cn/ScriptReference/BuildAssetBundleOptions.UncompressedAssetBundle.html) 时由 Unity 构建的未压缩 AssetBundle 无需解压缩，但是会占用更多磁盘空间。

### AssetBundle 缓存

为了使用 WWW 或 [UnityWebRequest](https://docs.unity.cn/ScriptReference/Networking.UnityWebRequest.html) (UWR) 来优化 LZMA AssetBundle 的提取、再压缩和版本控制，Unity 有两种缓存：

- **内存缓存**以 [UncompressedRuntime](https://docs.unity.cn/ScriptReference/BuildCompression.UncompressedRuntime.html) 格式将 AssetBundle 存储在 RAM 中。
- **磁盘缓存**将提取的 AssetBundle 以下文描述的压缩格式存储在可写介质中。

将 AssetBundle 加载到内存缓存中会耗用大量的内存。除非您特别希望频繁且快速地访问 AssetBundle 的内容，否则内存缓存的性价比可能不高。因此，应改用磁盘缓存。

如果向 UWR API 提供版本参数，Unity 会将 AssetBundle 数据存储在磁盘缓存中。如果没有提供版本参数，Unity 将使用内存缓存。版本参数可以是版本号或哈希。如果 [Caching.compressionEnabled](https://docs.unity.cn/ScriptReference/Caching-compressionEnabled.html) 设置为 true，则对于所有后续下载，Unity 会在将 AssetBundle 写入磁盘时应用 LZ4 压缩。它不会压缩缓存中的现有未压缩数据。如果 [Caching.compressionEnabled](https://docs.unity.cn/ScriptReference/Caching-compressionEnabled.html) 设置为 false，Unity 在将 AssetBundle 写入磁盘时不会应用压缩。

最初加载缓存的 LZMA AssetBundle 所花费的时间更长，因为 Unity 必须将存档重新压缩为目标格式。随后的加载将使用缓存版本。

[AssetBundle.LoadFromFile](https://docs.unity.cn/ScriptReference/AssetBundle.LoadFromFile.html) 或 [AssetBundle.LoadFromFileAsync](https://docs.unity.cn/ScriptReference/AssetBundle.LoadFromFileAsync.html) 始终对 LZMA AssetBundle 使用内存缓存，因此您应该使用 UWR API。如果无法使用 UWR API，您可以使用 [AssetBundle.RecompressAssetBundleAsync](https://docs.unity.cn/ScriptReference/AssetBundle.RecompressAssetBundleAsync.html) 将 LZMA AssetBundle 重写到磁盘中。

内部测试表明，使用磁盘缓存而不是内存缓存在 RAM 使用率方面至少存在一个数量级的差异。因此，必须在内存影响、增加的磁盘空间要求以及应用程序的资源实例化时间之间进行权衡。

## 资源重复

当对象构建到 AssetBundle 中时，Unity 5 的 AssetBundle 系统会查找对象的所有依赖项。这是使用资源数据库完成的。此依赖关系信息用于确定包含在 AssetBundle 中的对象集。

显式分配给 AssetBundle 的对象将仅构建到该 AssetBundle 中。当对象的 AssetImporter 将其 assetBundleName 属性设置为非空字符串时，表示“显式指定”该对象。

未显式分配到 AssetBundle 中的任何对象将包含在所有 AssetBundle 中，这些 AssetBundle 会包含一个或多个引用该未标记对象的对象。

如果将两个不同的对象分配给两个不同的 AssetBundle，但两者都引用了一个共同的依赖项对象，那么该依赖项对象将被复制到两个 AssetBundle 中。复制的依赖项也将被实例化，这意味着依赖项对象的两个副本将被视为具有不同标识符的不同对象。这将增加应用程序的 AssetBundle 的总大小。如果应用程序加载对象的两个父项，则还会导致将两个不同的对象副本加载到内存中。

通过组合 AssetDatabase 和 AssetImporter API，可以编写一个 Editor 脚本，确保将所有 AssetBundle 的直接或间接依赖项都分配给 AssetBundle，或者不会有两个 AssetBundle 共享尚未分配给 AssetBundle 的依赖项。由于复制资源的内存成本，建议所有项目都采用这样的脚本。

### 精灵图集重复

以下部分将介绍 Unity 5 的资源依赖性计算代码在与自动生成的精灵图集结合使用时出现的奇怪行为。

任何自动生成的精灵图集都将与生成精灵图集的精灵对象一起分配到同一个 AssetBundle。如果精灵对象被分配给多个 AssetBundle，则精灵图集将不会被分配给 AssetBundle 并且将被复制。如果精灵对象未分配给 AssetBundle，则精灵图集也不会分配给 AssetBundle。

为了确保精灵图集不重复，请确保标记到相同精灵图集的所有精灵都被分配到同一个 AssetBundle。

### Android 纹理

由于 Android 生态系统中存在严重的设备碎片，因此通常需要将纹理压缩为多种不同的格式。虽然所有 Android 设备都支持 ETC1，但 ETC1 不支持具有 Alpha 通道的纹理。如果应用程序不需要 OpenGL ES 2 支持，解决该问题的最简单方法是使用所有 Android OpenGL ES 3 设备都支持的 ETC2。

在运行时，可以使用 [SystemInfo.SupportsTextureFormat](http://docs.unity.cn/ScriptReference/SystemInfo.SupportsTextureFormat.html?_ga=1.141687282.1751468213.1479139860) API 检测对不同纹理压缩格式的支持情况。应使用此信息来选择和加载含有以受支持格式压缩的纹理的 AssetBundle 变体。

## 在运行时加载资源

Unity 支持项目中的__资源文件夹__，允许在主游戏文件中提供内容，但在请求之前不加载这些内容。也可以创建__资源包__。这些文件完全独立于主游戏文件，其中包含游戏按需从文件或 URL 访问的资源。

### 资源包 (Asset Bundle)

资源包是外部资源集合。您可以拥有许多资源包，因此可以拥有许多不同的外部资源集合。这些文件存在于构建的 Unity 播放器之外，通常位于 Web 服务器上，供最终用户动态访问。

要构建资源包，可在 Editor 脚本中调用 [BuildPipeline.BuildAssetBundles()](https://docs.unity.cn/cn/2019.4/ScriptReference/BuildPipeline.BuildAssetBundles.html)。在参数中，指定要包含在构建文件中的__对象__数组以及其他一些选项。这将构建一个文件，稍后即可使用 [AssetBundle.LoadAsset()](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetBundle.LoadAsset.html) 在运行时动态加载该文件。

### 资源文件夹 (Resources)

资源文件夹是包含在构建的 Unity 播放器中但不一定链接到 Inspector 中任何游戏对象的资源集合。

要将任何内容放入资源文件夹，只需在 __Project 视图__中创建一个新文件夹，并将该文件夹命名为“Resources”。可在项目中以不同方式组织多个资源文件夹。每当想从其中一个文件夹加载资源时，请调用 [Resources.Load()](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html)。

#### 注意：

在 Resources 文件夹中找到的所有资源及其依赖项都存储在名为 *resources.assets* 的文件中。如果一个资源已被另一个关卡使用，则该资源会存储在该关卡的 *.sharedAssets* 文件中。 **Edit > PlayerSettings** **First Streamed Level** 设置决定了在哪个关卡收集 *resources.assets* 并将其包含在构建中。

通过 [Resources.Load()](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html) 只能访问 _Resources 文件夹_中的资源。然而，更多资源可能最终出现在“resources.assets”文件中，因为它们是依赖项。（例如，Resources 文件夹中的材质可能引用 Resources 文件夹之外的纹理）

### 资源卸载

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

## 游戏对象

__游戏对象__是 Unity 中的基础对象，表示角色、道具和景物。它们本身并没有取得多大作为，但它们充当__组件__的容器，而组件可实现真正的功能。游戏对象始终附加一个[变换](https://docs.unity.cn/cn/2019.4/Manual/class-Transform.html)组件（表示位置和方向），并且无法删除此组件。

### 胶囊体

![img](https://docs.unity.cn/cn/2019.4/uploads/Main/PrimitiveCapsule.png)

胶囊体是一个两端有半球形帽的圆柱体。此对象的直径为一个单位且高度为两个单位（主体为一个单位，两个半球形帽各为半个单位）。胶囊体经过纹理化后使图像恰好环绕一次并收缩到每个半球的顶点。虽然真实世界中没有很多这种形状的对象，但胶囊体是可用于原型设计的占位对象。特别是对于某些任务，圆形对象的物理性质有时优于盒体。

### 圆柱体

![img](https://docs.unity.cn/cn/2019.4/uploads/Main/PrimitiveCylinder.png)

这是一个高度为两个单元且直径为一个单元的简单圆柱体，经过纹理化后使图像围绕主体的管状环绕一次，但也分别出现在两个扁平的圆形端。圆柱体非常适合用于制作柱子、杆和轮子，但应该注意碰撞体的形状实际上是一个胶囊体（Unity 中没有原始圆柱体碰撞体）。如果需要将精确的圆柱形碰撞体用于物理目的，应在建模程序中创建适当形状的网格，并附加网格碰撞体。

## 变换组件 (Transform)

**Transform** 组件确定每个对象在场景中的 **Position**、**Rotation** 和 **Scale** 属性的值。每个游戏对象都有一个变换组件。

![img](https://docs.unity.cn/cn/2019.4/uploads/Main/TransformExample4.png)

### 属性

| ***属性：*** | ***功能：***                                                 |
| :----------: | :----------------------------------------------------------- |
| **Position** | 变换组件在 X、Y 和 Z 坐标的位置。                            |
| **Rotation** | 变换组件围绕 X、Y 和 Z 轴的旋转（以度为测量单位）。          |
|  **Scale**   | 变换组件沿着 X、Y 和 Z 轴的缩放。值“1”表示原始大小（对象导入时的大小）。 |

变换组件的位置、旋转和缩放值是相对于变换组件的父项测量的。如果变换组件没有父项，则按照世界空间来测量属性。

## 使用脚本来创建组件

编写脚本（或创建脚本）是指使用 [Unity Scripting API](https://docs.unity.cn/cn/2019.4/Manual/CreatingAndUsingScripts.html) 以代码方式来自行编写 Unity Editor 的附加功能。

创建脚本并将脚本附加到__游戏对象__时，脚本将显示在__游戏对象__的 **Inspector** 中，就像内置__组件__一样。这是因为，将脚本保存到项目后，脚本也变成了__组件__。

从技术方面来说，编写的任何脚本都会编译为__组件__类型，因此 Unity Editor 会将脚本视为内置组件。可以定义要在 Inspector 中公开的脚本成员，并且 Editor 会执行您编写的任何功能。

## 停用游戏对象

可以通过将游戏对象标记为非活动来暂时从场景中移除此对象。要执行此操作，请导航到 Inspector 并取消选中该游戏对象名称旁边的复选框（请参阅下图），或使用脚本中的 [activeSelf](https://docs.unity.cn/cn/2019.4/ScriptReference/GameObject-activeSelf.html) 属性。

![游戏对象名称旁边的激活复选框，均在红色框中突出显示](https://docs.unity.cn/cn/2019.4/uploads/Main/GOActiveBox1.png)

游戏对象名称旁边的激活复选框，均在红色框中突出显示

### 停用父游戏对象

停用父游戏对象时，也会停用其所有子游戏对象。

此停用会覆盖所有子游戏对象上的 `activeSelf` 设置，因此 Unity 会使父级下的整个层级视图将变为非活动状态。这不会更改子游戏对象上 `activeSelf` 属性的值，因此重新激活父对象时，子对象将恢复到其原始状态。这意味着无法通过读取 **activeSelf** 属性来确定子游戏对象当前是否在场景中处于活动状态。而应该使用 [activeInHierarchy](https://docs.unity.cn/cn/2019.4/ScriptReference/GameObject-activeInHierarchy.html) 属性，该属性将考虑父对象的覆盖效果。

## 标签

**标签 (Tag)** 是可分配给一个或多个__游戏对象__的参考词。例如，可为玩家控制的角色定义“Player”标签，并为非玩家控制的角色定义“Enemy”标签。还可以使用“Collectable”标签定义玩家可在场景中收集的物品。

标签有助于识别游戏对象以便于编写脚本。通过使用标签，不需要使用拖放方式手动将游戏对象添加到脚本的公开属性，因此可以节省在多个游戏对象中使用相同脚本代码的时间。

标签对[碰撞体](https://docs.unity.cn/cn/2019.4/Manual/CollidersOverview.html)控制脚本中的触发器很有用；例如，需要通过标签确定玩家是否与敌人、道具或可收集物进行交互。

通过设置 [GameObject.FindWithTag()](https://docs.unity.cn/cn/2019.4/ScriptReference/GameObject.FindWithTag.html) 函数，可以查找包含所需标签的游戏对象。

### 提示

- 只能为游戏对象分配一个标签。
- Unity 包含一些未出现在标签管理器中的内置标签：
  - **Untagged**
  - **Respawn**
  - **Finish**
  - **EditorOnly**
  - **MainCamera**
  - **Player**
  - **GameController**
- 可以使用任何喜欢的词作为标签。

## 静态游戏对象

如果游戏对象在运行时未移动，则被称为**静态游戏对象**。如果游戏对象在运行时移动，则被称为**动态游戏对象**。

Unity 中的许多系统都可以在 Editor 中预计算有关静态游戏对象的信息。由于静态游戏对象不会移动，因此这些计算的结果在运行时仍然有效。这意味着 Unity 可以节省运行时计算成本，并可能提高性能。

可以使用 [GameObjectUtility.SetStaticEditorFlags](https://docs.unity.cn/cn/2019.4/ScriptReference/GameObjectUtility.SetStaticEditorFlags.html) API 和 [GameObject.isStatic](https://docs.unity.cn/cn/2019.4/ScriptReference/GameObject.isStatic.html) 在代码中设置 Static Editor Flags 属性。

## 保存工作

__脏资源__：Unity 还会保存所有标记为**脏**的资源（表示已触碰或修改过资源）。可以使用[自定义编辑器](https://docs.unity.cn/cn/2019.4/Manual/editor-CustomEditors.html)和[脚本](https://docs.unity.cn/cn/2019.4/Manual/CreatingAndUsingScripts.html)按以下方式之一来保存脏资源：

- 使用 [SerializedObject](https://docs.unity.cn/cn/2019.4/ScriptReference/SerializedObject.html) 类以及 [SerializedProperties](https://docs.unity.cn/cn/2019.4/ScriptReference/SerializedProperty.html)。
- 使用 [Undo](https://docs.unity.cn/cn/2019.4/ScriptReference/Undo.html) 类来记录修改。
- 如果以上方法均未奏效，还可以使用 [SetDirty](https://docs.unity.cn/cn/2019.4/ScriptReference/EditorUtility.SetDirty.html)。