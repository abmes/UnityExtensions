using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public static class FactoryFuncExtensionMethods
    {
        public static IUnityContainer RegisterTypeByFactoryFunc<TResult>(this IUnityContainer container, Func<TResult> factoryFunc) where TResult : class
        {
            return container.RegisterTypeEx<TResult>().ByFactoryFunc(factoryFunc);
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1>(this IUnityContainer container, Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeEx<TResult>().ByFactoryFunc<TParam1>(factoryFunc, resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeEx<TResult>().ByFactoryFunc<TParam1, TParam2>(factoryFunc, resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeEx<TResult>().ByFactoryFunc<TParam1, TParam2, TParam3>(factoryFunc, resolvedParameters);
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2, TParam3, TParam4>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters) where TResult : class
        {
            return container.RegisterTypeEx<TResult>().ByFactoryFunc<TParam1, TParam2, TParam3, TParam4>(factoryFunc, resolvedParameters);
        }
    }
}
