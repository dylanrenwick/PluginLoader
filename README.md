`SimplePlugin` is a standalone plugin with no dependencies.
The test for `SimplePlugin` confirms that:
 - The plugin is loaded
 - The plugin contains a type `Class1`
 - The type has a method `Run()`
 - The original file is _not_ locked while the plugin is loaded

`DependentPlugin` is a plugin with dependencies. It depends on both the local project `Dependency`, and the nuget package `Newtonsoft.Json`.  
The local project `Dependency` is also directly referenced by the test suite. The nuget package is not.
The test for `DependentPlugin` confirms that:
 - The plugin is loaded
 - The plugin contains a type `Class1`
 - The type implements an interface `IDependency`
 - The type has a method `Run()`
 - The class has the attribute `JsonObjectAttribute` from the `Newtonsoft.Json` package
 - The original file and dependency file are _not_ locked while the plugin is loaded
