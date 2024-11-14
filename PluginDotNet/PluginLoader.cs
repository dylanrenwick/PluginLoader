using System.Reflection;
using System.Runtime.Loader;

namespace PluginDotNet;

public class PluginLoader
{
    private const string _shadowDirectory = "_shadow";

    private readonly string _assemblyRoot;
    private readonly string _shadowPath;

    private readonly Dictionary<int, AssemblyLoadContext> _plugins = [];

    public PluginLoader()
    {
        _assemblyRoot = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!;
        _shadowPath = Path.Combine(_assemblyRoot, _shadowDirectory);

        if (Directory.Exists(_shadowPath))
            Directory.Delete(_shadowPath, true);
        Directory.CreateDirectory(_shadowPath);
    }

    public (int, Assembly) LoadAssembly(string filePath)
    {
        AssemblyLoadContext loadContext = new(Path.GetFileName(filePath), true);
        File.Copy(filePath, Path.Combine(_shadowPath, Path.GetFileName(filePath)), true);
        string shadowPath = Path.Combine(_shadowPath, Path.GetFileName(filePath));
        Assembly asm = loadContext.LoadFromAssemblyPath(shadowPath);
        int id = _plugins.Count;
        _plugins.Add(id, loadContext);
        return (id, asm);
    }

    public void UnloadAssembly(int id)
    {
        if (!_plugins.ContainsKey(id))
            throw new Exception("invalid id");

        AssemblyLoadContext ctx = _plugins[id];
        _plugins.Remove(id);
        ctx.Unload();
        ctx = null;
    }
}
