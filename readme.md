Abmes.UnityExtensions
=====================
[![abmes MyGet Build Status](https://www.myget.org/BuildSource/Badge/abmes?identifier=2f697fe1-acf4-44c9-841e-528fba65a13e)](https://www.myget.org/)

This project provides some extensions for the [Unity](http://unity.codeplex.com/) IoC container.

AutoFactory extensions
----------------------

This is an extension method for registering auto factories using the [Unity.TypedFactories](https://github.com/PombeirP/Unity.TypedFactories) project.
It wraps the call with generic type arguments and returns the container so that its fluent interface can continue.

Instead of writing
```c#
container.RegisterTypedFactory<IFooFactory>().ForConcreteType<Foo>();
```
you can write
```c#
container.RegisterTypeEx<IFooFactory>().AsAutoFactoryFor<Foo>();
```
or the more compact form
```c#
container.RegisterTypeAsAutoFactory<IFooFactory, Foo>();
```
RegisterTypeSingleton extensions
--------------------------------

These are extension methods which pass the ContainerControlledLifetimeManager to the container so instead of writing
```c#
container.RegisterType<IFoo, Foo>(new ContainerControlledLifetimeManager());
```
you can write
```c#
container.RegisterTypeSingleton<IFoo, Foo>();
```

IEnumerable extensions
----------------------

With Unity a consumer can expect an array of objects implementing an interface.
Unity will resolve all **named** registrations and pass them as an array to the consumer.
```c#
container.RegisterType<ILoader, FooLoader>("foo");
container.RegisterType<ILoader, BarLoader>("bar");
container.RegisterType<ILoader, BazLoader>("baz");

public MyConsumer
{
	public MyConsumer(ILoader[] loaders)
	{
		...
	}
}

var c = container.Resolve<MyConsumer>();
```

However this will not work if the consumer expects an IEnumerable of the interface
```c#
	public MyConsumer(IEnumerable<ILoader> loaders)
```

to fix this you can call the extension method **RegisterIEnumerable** on the container
```c#
container.RegisterIEnumerable();
```

ByFactoryFunc extensions
------------------------

With unity you can use an InjectionFactory to resolve a given type like this
```c#
container.RegisterType<IFoo>(new InjectionFactory(c => c.Resolve<IFooFactory>().CreateFoo()));
```

We have added a new type **InjectionParameterizedFactory** which resolves the requested types and passes them to the given factory function
```c#
Func<IFooFactory, IFoo> factoryFunc = fooFactory => fooFactory.CreateFoo();

container.RegisterType<IFoo>(new InjectionParameterizedFactory(factoryFunc));
```
The above code will resolve all the parameters of the *factoryFunc* and then call it passing them in to the function to produce the result of the resolving.
You can read the above two lines as
* define that you can get an IFoo by calling CreateFoo() on an IFooFactory
* tell the container to use the above definition whenever it needs an IFoo

For convenience we have added extension methods with generic type arguments to define the function and register it with the container.
```c#
container.RegisterTypeEx<IFoo>().ByFactoryFunc<IFooFactory>(fooFactory => fooFactory.CreateFoo());
```
and the more compact form
```c#
container.RegisterTypeByFactoryFunc<IFoo, IFooFactory>(fooFactory => fooFactory.CreateFoo());
```
The above code can be read as follows "An IFoo can be obtained by calling CreateFoo() on an IFooFactory"

These can get more complicated like the following example
```c#
container.RegisterTypeEx<IStat>().ByFactoryFunc<IStatReport, IStatReportParser>((report, parser) => parser.ParseReport(report));
```
and the more compact form
```c#
container.RegisterTypeByFactoryFunc<IStat, IStatReport, IStatReportParser>((report, parser) => parser.ParseReport(report));
```
The above code can be read as follows "An IStat can be obtained by parsing an IStatReport with an IStatReportParser".

ByFactoryFuncLazy extensions
----------------------------

Sometimes you may need lazy instantiation of the implementation. Then you can use
```c#
container.RegisterTypeEx<IFoo>().ByFactoryFuncLazy<IFooFactory>(fooFactory => fooFactory.CreateFoo());
```
or the more compact form
```c#
container.RegisterTypeByFactoryFuncLazy<IFoo, IFooFactory>(fooFactory => fooFactory.CreateFoo());
```
This way the container will give you a proxy object implementing IFooFactory which will actually create the underlying object when you call CreateFoo().
There is no RegisterTypeLazy<IFoo, Foo>() extension method because you only need something to be lazy if some work will be done and no work should be done in a constructor.

Decorator pattern extensions
----------------------------

If you want to use the decorator pattern with Unity you may find yourself writing something like
```c#
container.RegisterType<ILogger, Logger>();
container.RegisterType<ICacheManager, SimpleCache>();

container.RegisterType<ITenantStore, TenantStore>("BasicStore");
container.RegisterType<ITenantStore, LoggingTenantStore>("LoggingStore",
    new InjectionConstructor(
        new ResolvedParameter<ITenantStore>("BasicStore"),
        new ResolvedParameter<ILogger>()));

// Default registration
container.RegisterType<ITenantStore, CachingTenantStore>(
    new InjectionConstructor(
        new ResolvedParameter<ITenantStore>("LoggingStore"),
        new ResolvedParameter<ICacheManager>()));
```

Which is quite cumbersome.
If you use the EnableDecoration() extension method you can do it as simple as
```c#
container
    .EnableDecoration()

    .RegisterType<ILogger, Logger>()
    .RegisterType<ICacheManager, SimpleCache>()

    .RegisterType<ITenantStore, CachingTenantStore>()
    .RegisterType<ITenantStore, LoggingTenantStore>()
    .RegisterType<ITenantStore, TenantStore>();
```
just like in Castle.Windsor

A few things you should know when using the EnableDecoration() extension method:
* The order is important. The first registration is used to satisfy the dependency. If it has a dependency on the same interface the next registration is used to satisfy it.
* A type in the decorator chain can have other dependencies and they will be satisfied by the container
* You can include a RegisterTypeByFactoryFunc in the decorator chain
* **Very Important**. You must use the container returned by .EnableDecoration() for this extension to work. You should forget about the original container after you call .EnableDecoration(). We suggest doing something like
```c#
var container = new UnityContainer().EnableDecoration();
container.RegisterType<IFoo, Foo>();
...
```
or
```c#
public IUnityContainer GetContainerWithRegistrations()
{
    return new UnityContainer()
        .EnableDecoration()
        .RegisterType<IFoo, Foo>();
}
```

Configuraton From Surrounding Assemblies
----------------------------------------

If you want to configire registrations in the UnityContainer from the surrounding assemblies (referenced or not) you can use the **ConfigureFromSurroundingAssemblies()** extension method.

It loads all surrounding assemblies, searches for public types implementing the **IUnityContainerConfigurator** interface and calls RegisterTypes() on each one of them.

So if you want to register types in a global container from an assembly you should create a class implementing the **IUnityContainerConfigurator** interface and register your types there.

Sample usage:

**Main Assembly**
```c#
container.ConfigureFromSurroundingAssemblies();
```

**Surrounding Assembly**
```c#
public class UnityContainerConfigurator : IUnityContainerConfigurator
{
	public void RegisterTypes(IUnityContainer container)
	{
		container.RegisterType<IFoo, Foo>();
	}
}
```
