using System.Reflection;

namespace PluginDotNet.Test;

public class PluginLoaderTests : IClassFixture<PluginLoader>
{
    private const string _simplePluginPath = "../../../../SimplePlugin/bin/Debug/net8.0/SimplePlugin.dll";
    private const string _dependentPluginPath = "../../../../DependentPlugin/bin/Debug/net8.0/DependentPlugin.dll";
    private const string _dependencyPath = "../../../../DependentPlugin/bin/Debug/net8.0/Dependency.dll";

    private readonly PluginLoader _loader;

    public PluginLoaderTests(PluginLoader loader)
    {
        _loader = loader;
    }

    [Fact]
    public void LoadAssembly_SimplePlugin_ProvidesAssembly()
    {
        string absPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!, _simplePluginPath);
        Assembly simplePlugin = _loader.LoadAssembly(absPath);

        Assert.NotNull(simplePlugin);
        Assert.False(FileUtil.IsLocked(_simplePluginPath));

        Type? type = simplePlugin.ExportedTypes.FirstOrDefault(t => t.Name == "Class1");
        Assert.NotNull(type);

        object? instance = Activator.CreateInstance(type!);
        Assert.NotNull(instance);

        type.GetMethod("Run")?.Invoke(instance, null);
    }

    [Fact]
    public void LoadAssembly_DependentPlugin_ProvidesAssembly()
    {
        string absPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!, _dependentPluginPath);
        Assembly dependentPlugin = _loader.LoadAssembly(absPath);

        Assert.NotNull(dependentPlugin);
        Assert.False(FileUtil.IsLocked(_dependentPluginPath));
        Assert.False(FileUtil.IsLocked(_dependencyPath));

        Type? type = dependentPlugin.ExportedTypes.FirstOrDefault(t => t.Name == "Class1");
        Assert.NotNull(type);
        Assert.True(type.IsAssignableTo(typeof(Dependency.IDependency)));

        var attributes = type.GetCustomAttributes();
        Assert.Contains(attributes, attr => attr.GetType().FullName == "Newtonsoft.Json.JsonObjectAttribute");

        object? instance = Activator.CreateInstance(type!);
        Assert.NotNull(instance);

        type.GetMethod("Run")?.Invoke(instance, null);
    }
}