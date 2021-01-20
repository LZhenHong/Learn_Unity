# Unity 特殊文件夹名称

## Assets

**Assets** 文件夹是包含 Unity 项目使用的资源的主文件夹。Editor 中的 Project 窗口的内容直接对应于 Assets 文件夹的内容。大多数 API 函数都假定所有内容都位于 Assets 文件夹中，因此不要求显式提及该文件夹。但是，有些函数需要将 Assets 文件夹作为路径名的一部分添加（例如，[AssetDatabase](https://docs.unity.cn/cn/2019.4/ScriptReference/AssetDatabase.html) 类中的一些函数）。

## Editor

放在名为 **Editor** 的文件夹中的脚本被视为 Editor 脚本而不是运行时脚本。这些脚本在开发期间向 Editor 添加功能，并在运行时在构建中不可用。

可在 Assets 文件夹中的任何位置添加多个 Editor 文件夹。应将 Editor 脚本放在 Editor 文件夹内或其中的子文件夹内。

Editor 文件夹的具体位置会影响其脚本相对于其他脚本的编译时间（有关此方面的完整说明，请参阅[特殊文件夹和脚本编译顺序](https://docs.unity.cn/cn/2019.4/Manual/ScriptCompileOrderFolders.html)的相关文档）。使用 Editor 脚本中的 [EditorGUIUtility.Load](https://docs.unity.cn/cn/2019.4/ScriptReference/EditorGUIUtility.Load.html) 函数可从 Editor 文件夹中的 Resources 文件夹加载资源。这些资源只能通过 Editor 脚本加载，并会从构建中剥离。

**注意：**如果脚本位于 Editor 文件夹中，Unity 不允许将派生自 MonoBehaviour 的组件分配给游戏对象。

## Gizmos

[Gizmos](https://docs.unity.cn/cn/2019.4/ScriptReference/Gizmos.html) 允许将图形添加到 Scene 视图，以帮助可视化不可见的设计细节。[Gizmos.DrawIcon](https://docs.unity.cn/cn/2019.4/ScriptReference/Gizmos.DrawIcon.html) 函数在场景中放置一个图标，作为特殊对象或位置的标记。必须将用于绘制此图标的图像文件放在名为 **Gizmos** 的文件夹中，这样才能被 DrawIcon 函数找到。

只能有一个 Gizmos 文件夹，且必须将其放在项目的根目录；直接位于 Assets 文件夹中。将所需的资源文件放在此 Gizmos 文件夹内或其中的子文件夹内。如果资源文件位于子文件夹中，请始终在传递给 [Gizmos.DrawIcon](https://docs.unity.cn/cn/2019.4/ScriptReference/Gizmos.DrawIcon.html) 函数的路径中包含子文件夹路径。

## Resources

可从脚本中按需加载资源，而不必在场景中创建资源实例以用于游戏。为此，应将资源放在一个名为 **Resources** 的文件夹中。通过使用 [Resources.Load](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html) 函数即可加载这些资源。

可在 Assets 文件夹中的任何位置添加多个 Resources 文件夹。将所需的资源文件放在 Resources 文件夹内或其中的子文件夹内。如果资源文件位于子文件夹中，请始终在传递给 [Resources.Load](https://docs.unity.cn/cn/2019.4/ScriptReference/Resources.Load.html) 函数的路径中包含子文件夹路径。

请注意，如果 Resources 文件夹是 Editor 的子文件夹，则其中的资源可通过 Editor 脚本加载，但会从构建中剥离。

## StreamingAssets

尽管将资源直接合并到构建中更为常见，但有时可能希望资源以其原始格式作为单独的文件提供。例如，需要从文件系统访问视频文件，而不是用作 MovieTexture 在 iOS 上播放该视频。将一个文件放在名为 **StreamingAssets** 的文件夹中，这样就会将其按原样复制到目标计算机，然后就能从特定文件夹中访问该文件。请参阅关于[流媒体资源 (Streaming Assets)](https://docs.unity.cn/cn/2019.4/Manual/StreamingAssets.html) 的页面以了解更多详细信息。

只能有一个 StreamingAssets 文件夹，且必须将其放在项目的根目录；直接位于 Assets 文件夹中。将所需的资源文件放在此 StreamingAssets 文件夹内或其中的子文件夹内。如果资源文件位于子文件夹中，请始终在用于引用流媒体资源的路径中包含子文件夹路径。