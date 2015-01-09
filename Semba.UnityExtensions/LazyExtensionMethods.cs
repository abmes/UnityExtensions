using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Semba.UnityExtensions
{
    public static class LazyExtensionMethods
    {
        class LazyInterceptor<T> : Castle.DynamicProxy.IInterceptor
        {
            private readonly Lazy<T> _lazyObject;

            public LazyInterceptor(Func<T> factoryFunc)
            {
                _lazyObject = new Lazy<T>(factoryFunc);
            }

            public void Intercept(Castle.DynamicProxy.IInvocation invocation)
            {
                var targetType = typeof(T);
                var method = targetType.GetMethod(invocation.Method.Name, invocation.Method.GetParameters().Select(x => x.ParameterType).ToArray());
                invocation.ReturnValue = method.Invoke(_lazyObject.Value, invocation.Arguments);
            }
        }

        // ------------------------------

        //public static IUnityContainer RegisterLazyDecorator<TFrom>(this IUnityContainer container) where TFrom : class
        //{
        //    var proxyGenerator = new Castle.DynamicProxy.ProxyGenerator();

        //    var toType = proxyGenerator.ProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(typeof(TFrom), null, null);
        //    //var toType = proxyGenerator.CreateInterfaceProxyWithoutTarget<TFrom>(new LazyInterceptor<TFrom>(container.Resolve<Func<TFrom>>()));

        //    return container.RegisterType(typeof(TFrom), toType, new InjectionFactory(c => proxyGenerator.CreateInterfaceProxyWithoutTarget<TFrom>(new LazyInterceptor<TFrom>(c.Resolve<Func<TFrom>>()))));
        //}

        // -------------------------------

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult>(this IUnityContainer container, Func<TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new Castle.DynamicProxy.ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc()))));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam>(this IUnityContainer container, Func<TParam, TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new Castle.DynamicProxy.ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc(c.Resolve<TParam>())))));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, TParam2, TResult> factoryFunc) where TResult : class
        {
            return
                container.RegisterType<TResult>(
                    new InjectionFactory(c =>
                        {
                            var param1Func = c.Resolve<Func<TParam1>>();
                            var param2Func = c.Resolve<Func<TParam2>>();

                            return
                                new Castle.DynamicProxy.ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(
                                    new LazyInterceptor<TResult>(() => factoryFunc(param1Func(), param2Func())));
                        }));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new Castle.DynamicProxy.ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc(c.Resolve<TParam1>(), c.Resolve<TParam2>(), c.Resolve<TParam3>())))));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2, TParam3, TParam4>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc) where TResult : class
        {
            return container.RegisterType<TResult>(new InjectionFactory(c => new Castle.DynamicProxy.ProxyGenerator().CreateInterfaceProxyWithoutTarget<TResult>(new LazyInterceptor<TResult>(() => factoryFunc(c.Resolve<TParam1>(), c.Resolve<TParam2>(), c.Resolve<TParam3>(), c.Resolve<TParam4>())))));
        }
    }
}