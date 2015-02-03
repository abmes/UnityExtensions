Abmes.UnityExtensions
====================

This project provides some extensions for the [Unity](http://unity.codeplex.com/) IoC container.

AutoFactory extensions
----------------------

This is an extension method for registering auto factories using the [Unity.TypedFactories](https://github.com/PombeirP/Unity.TypedFactories) project.
It wraps the call with generic type arguments and returns the container so that its fluent interface can continue.

Instead of writing
```C#
container.RegisterTypedFactory<IFooFactory>().ForConcreteType<Foo>();
```
you can write
```C#
container.RegisterAutoFactory<IFooFactory, Foo>();
```
RegisterTypeSingleton extensions
--------------------------------

These are extension methods which pass the ContainerControlledLifetimeManager to the container so instead of writing
```C#
container.RegisterType<IFoo, Foo>(new ContainerControlledLifetimeManager());
```
you can write
```C#
public class Test {}
container.RegisterTypeSingleton<IFoo, Foo>();
```