using Dependency;
using Newtonsoft.Json;

namespace DependentPlugin;

[JsonObject]
public class Class1 : IDependency
{
    public void Run()
    {
        Console.WriteLine("DependentPlugin.Class1.Run() called.");
    }
}
