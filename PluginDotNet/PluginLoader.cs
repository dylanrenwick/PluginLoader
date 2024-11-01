using System.Reflection;
using System.Runtime.Loader;

namespace PluginDotNet;

public class PluginLoader
{
    private const string _shadowDirectory = "_shadow";

    private readonly string _assemblyRoot;
    private readonly string _shadowPath;

    public PluginLoader()
    {
        _assemblyRoot = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!;
        _shadowPath = Path.Combine(_assemblyRoot, _shadowDirectory);

        if (Directory.Exists(_shadowPath))
            Directory.Delete(_shadowPath, true);
        Directory.CreateDirectory(_shadowPath);
    }

    public Assembly LoadAssembly(string filePath)
    {
        File.Copy(filePath, Path.Combine(_shadowPath, Path.GetFileName(filePath)), true);
        string shadowPath = Path.Combine(_shadowPath, Path.GetFileName(filePath));
        return Assembly.LoadFrom(shadowPath);
    }
}
