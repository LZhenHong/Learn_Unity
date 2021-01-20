# Unity 与 VS

1. 尽管 Visual Studio 附带了自己的 C# 编译器，并且您可以使用它来检查 # 脚本中是否存在错误，但 Unity 仍然使用自己的 C# 编译器来编译脚本。使用 Visual Studio 编译器仍然非常有用，因为这意味着不必一直切换到 Unity 来检查是否有任何错误。
2. Visual Studio 的 C# 编译器比 Unity 的 C# 编译器目前支持的功能更多。也就是说，某些代码（尤其是较新的 # 功能）不会在 Visual Studio 中抛出错误，但在 Unity 中则会。
3. **Unity 会自动创建和维护 Visual Studio .sln 和 .csproj 文件。每当在 Unity 中添加/重命名/移动/删除文件时，Unity 都会重新生成 .sln 和 .csproj 文件。**也可以从 Visual Studio 向解决方案添加文件。Unity 随后会导入这些新文件，下次 Unity 再次创建项目文件时，便会使用包含的新文件进行创建。

