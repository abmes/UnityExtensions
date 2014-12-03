using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Reflection.Emit;
using System.Reflection;
using System.Threading;
using Castle.DynamicProxy;
using Unity.TypedFactories;

namespace Semba.UnityExtensions
{
    public static class UnityContainerExtensionMethods
    {
        public static IUnityContainer RegisterTypeSingleton<TFrom, TTo>(this IUnityContainer container, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            return container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterTypeSingleton<T>(this IUnityContainer container, params InjectionMember[] injectionMembers)
        {
            return container.RegisterType<T>(new ContainerControlledLifetimeManager(), injectionMembers);
        }

        // -------------------------------

        public static IUnityContainer RegisterIEnumerable(this IUnityContainer container)
        {
            return container.RegisterType(typeof(IEnumerable<>), new InjectionFactory((c, t, n) => c.ResolveAll(t.GetGenericArguments().Single())));
        }

        // -------------------------------

        public static IUnityContainer RegisterDecoratorChain<TFrom>(this IUnityContainer container, LifetimeManager lifetimeManager, params Type[] decoratorChain)
        {
            string previousRegistrationName = null;

            foreach (var t in decoratorChain)
            {
                var currentRegistrationName = (t == decoratorChain.Last()) ? "" : Guid.NewGuid().ToString();
                var currentLifetimeManager = (t == decoratorChain.Last()) ? lifetimeManager : new TransientLifetimeManager();
                var constructor = t.GetConstructors().Single();
                var parameters = constructor.GetParameters().Select(x => x.ParameterType == typeof(TFrom) ? new ResolvedParameter<TFrom>(previousRegistrationName) : new ResolvedParameter(x.ParameterType)).ToArray();
                container.RegisterType(typeof(TFrom), t, currentRegistrationName, currentLifetimeManager, new InjectionConstructor(parameters));
                previousRegistrationName = currentRegistrationName;
            }

            return container;
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom>(this IUnityContainer container, params Type[] decoratorChain)
        {
            return container.RegisterDecoratorChain<TFrom>(new TransientLifetimeManager(), decoratorChain);
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator>(this IUnityContainer container, LifetimeManager lifetimeManager)
            where TTo : TFrom
            where TDecorator : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(lifetimeManager, typeof(TTo), typeof(TDecorator));
        }

        public static IUnityContainer RegisterSingletonDecoratorChain<TFrom, TTo, TDecorator>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(new ContainerControlledLifetimeManager(), typeof(TTo), typeof(TDecorator));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator1), typeof(TDecorator2));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2>(this IUnityContainer container, LifetimeManager lifetimeManager)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(lifetimeManager, typeof(TTo), typeof(TDecorator1), typeof(TDecorator2));
        }

        public static IUnityContainer RegisterSingletonDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(new ContainerControlledLifetimeManager(), typeof(TTo), typeof(TDecorator1), typeof(TDecorator2));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2, TDecorator3>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator3 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator1), typeof(TDecorator2), typeof(TDecorator3));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2, TDecorator3>(this IUnityContainer container, LifetimeManager lifetimeManager)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator3 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(lifetimeManager, typeof(TTo), typeof(TDecorator1), typeof(TDecorator2), typeof(TDecorator3));
        }

        public static IUnityContainer RegisterSingletonDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2, TDecorator3>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator3 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(new ContainerControlledLifetimeManager(), typeof(TTo), typeof(TDecorator1), typeof(TDecorator2), typeof(TDecorator3));
        }

        // -------------------------------

        public static IUnityContainer RegisterTypedFactory<TFactoryInterface, TConcreteType>(this IUnityContainer container) where TFactoryInterface : class
        {
            container.RegisterTypedFactory<TFactoryInterface>().ForConcreteType<TConcreteType>();
            return container;
        }

        // -------------------------------

        class LazyInterceptor<T> : IInterceptor
        {

            private readonly Lazy<T> _lazyObject;

            public LazyInterceptor(Func<T> factoryFunc)
            {
                _lazyObject = new Lazy<T>(factoryFunc);
            }

            public void Intercept(IInvocation invocation)
            {
                var targetType = typeof(T);
                var method = targetType.GetMethod(invocation.Method.Name, invocation.Method.GetParameters().Select(x => x.ParameterType).ToArray());
                invocation.ReturnValue = method.Invoke(_lazyObject.Value, invocation.Arguments);
            }
        }

        // ------------------------------

        public static IUnityContainer RegisterTypeLazy<TFrom, TTo>(this IUnityContainer container)
            where TFrom : class
            where TTo : class
        {
            return container.RegisterType<TFrom>(new InjectionFactory(c => new ProxyGenerator().CreateInterfaceProxyWithoutTarget<TFrom>(new LazyInterceptor<TTo>(() => c.Resolve<TTo>()))));
        }

        // -------------------------------

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult>(this IUnityContainer container, Func<TResult> factoryFunc)
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => factoryFunc()));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult>(this IUnityContainer container, Func<TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc()))));
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam>(this IUnityContainer container, Func<TParam, TResult> factoryFunc)
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => factoryFunc(c.Resolve<TParam>())));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam>(this IUnityContainer container, Func<TParam, TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc(c.Resolve<TParam>())))));
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, TParam2, TResult> factoryFunc)
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => factoryFunc(c.Resolve<TParam1>(), c.Resolve<TParam2>())));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, TParam2, TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc(c.Resolve<TParam1>(), c.Resolve<TParam2>())))));
        }

        //public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, Func<TParam2, TResult>> factoryFunc)
        //{
        //    return container.RegisterTypeByFactoryFunc<TResult, TParam1>(x => factoryFunc(x)(container.Resolve<TParam2>()));
        //}

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TResult> factoryFunc)
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => factoryFunc(c.Resolve<TParam1>(), c.Resolve<TParam2>(), c.Resolve<TParam3>())));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc(c.Resolve<TParam1>(), c.Resolve<TParam2>(), c.Resolve<TParam3>())))));
        }

        //public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, Func<TParam2, Func<TParam3, TResult>>> factoryFunc)
        //{
        //    return container.RegisterTypeByFactoryFunc<TResult, TParam1, TParam2>(x => y => factoryFunc(x)(y)(container.Resolve<TParam3>()));
        //}
        
        // -------------------------------

        //public static IUnityContainer RegisterAutoFactory(this IUnityContainer container, Type factoryInterface, Type createdType)
        //{
        //    if (!factoryInterface.IsInterface)
        //    {
        //        throw new ArgumentException("The passed type is not an interface", "factoryInterface");
        //    }

        //    var method = factoryInterface.GetMethods().Single();

        //    //assert createdtype is method.ReturnType

        //    //assert method.parameters == createdType.Constructor.Single.parameters

        //    AssemblyName assemblyName = new AssemblyName("DataBuilderAssembly");
        //    AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        //    var moduleBuilder = assemblyBuilder.DefineDynamicModule("ModuleName");
        //    var factoryTypeBuilder = moduleBuilder.DefineType("TypeName");
        //    factoryTypeBuilder.AddInterfaceImplementation(factoryInterface);
        //    var methodBuilder = factoryTypeBuilder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual, method.ReturnType, method.GetParameters().Select(x => x.ParameterType).ToArray());

        //    var il = methodBuilder.GetILGenerator();

        //    // da map-wa parametrite po ime i tip, a lipswashtite da gi deklarira kato dependency-ta prez konstruktor, koito konteinera shte resolve-ne i da si gi zapomnq
        //    var parameters = method.GetParameters();
        //    for (var i = 0; i < parameters.Count(); i++)
        //    {
        //        il.Emit(OpCodes.Ldarg, i + 1);
        //    }

        //    {
        //        return new createdType(param1, param2, ...);
        //    }
        //    il.Emit(OpCodes.Newobj, createdType.GetConstructors().Single());
        //    il.Emit(OpCodes.Ret);

        //    var factoryType = factoryTypeBuilder.CreateType();

        //    container.RegisterType(factoryInterface, factoryType);

        //    return container;
        //}
    }
}