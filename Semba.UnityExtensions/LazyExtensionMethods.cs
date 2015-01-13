using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Semba.UnityExtensions
{
    public static class LazyExtensionMethods
    {
        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult>(this IUnityContainer container, Func<TResult> factoryFunc) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult>(() => LazyProxy.GetLazyProxy(() => factoryFunc()));
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1>(this IUnityContainer container, Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>>((param1Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func())), resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>, Func<TParam2>>((param1Func, param2Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func())), resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>, Func<TParam2>, Func<TParam3>>((param1Func, param2Func, param3Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func(), param3Func())), resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFuncLazy<TResult, TParam1, TParam2, TParam3, TParam4>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeByFactoryFunc<TResult, Func<TParam1>, Func<TParam2>, Func<TParam3>, Func<TParam4>>((param1Func, param2Func, param3Func, param4Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func(), param3Func(), param4Func())), resolvedParameters);
        }
    }
}