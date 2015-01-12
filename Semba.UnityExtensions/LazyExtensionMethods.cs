using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Semba.UnityExtensions
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
            invocation.ReturnValue = invocation.Method.Invoke(_lazyObject.Value, invocation.Arguments);
        }
    }

    public static class LazyExtensionMethods
    {
        private static T GetLazyProxy<T>(Func<T> factoryFunc) where T : class
        {
            return new Castle.DynamicProxy.ProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(new LazyInterceptor<T>(factoryFunc));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult>(this IUnityContainer container, Func<TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult>(() => GetLazyProxy<TResult>(() => factoryFunc()) , resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1>(this IUnityContainer container, Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>>((param1Func) => GetLazyProxy<TResult>(() => factoryFunc(param1Func())), resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>, Func<TParam2>>((param1Func, param2Func) => GetLazyProxy<TResult>(() => factoryFunc(param1Func(), param2Func())), resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>, Func<TParam2>, Func<TParam3>>((param1Func, param2Func, param3Func) => GetLazyProxy<TResult>(() => factoryFunc(param1Func(), param2Func(), param3Func())), resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2, TParam3, TParam4>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>, Func<TParam2>, Func<TParam3>, Func<TParam4>>((param1Func, param2Func, param3Func, param4Func) => GetLazyProxy<TResult>(() => factoryFunc(param1Func(), param2Func(), param3Func(), param4Func())), resolvedParameters);
        }
    }
}