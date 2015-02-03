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
container.RegisterAutoFactory<IFooFactory, Foo>();
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
* define that given an IFooFactory you can get IFoo by calling its CreateFoo() method
* tell the container to use the above definition whenever it needs an IFoo

For convenience we have added extension methods with generic type arguments to define the function and register it with the container.
```c#
container.RegisterTypeByFactoryFunc<IFoo, IFooFactory>(fooFactory => fooFactory.CreateFoo());
```
The above code can be read as follows "When you need an IFoo resolve me an IFooFactory and call CreateFoo() on it to get the IFoo"

These can get more complicated like the following example
```c#
container.RegisterTypeByFactoryFunc<IStat, IReport, IReportParser>((report, parser) => parser.ParseReport(report));
```
The above code can be read as follows "When you need and IStat give me an IReport and an IReportParser then I can give you the IStat by parsing the report with the parser".

ByFactoryFuncLazy extensions
----------------------------

Decorator pattern extensions
----------------------------
