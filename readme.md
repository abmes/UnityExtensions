Abmes.UnityExtensions
====================

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