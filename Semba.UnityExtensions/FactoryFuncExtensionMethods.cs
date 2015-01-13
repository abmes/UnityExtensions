using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
{
    public static class FactoryFuncExtensionMethods
    {
        public static IUnityContainer RegisterTypeByFactoryFunc<TResult>(this IUnityContainer container, Func<TResult> factoryFunc)
        {
            return container.RegisterType<TResult>(new InjectionParameterizedFactory<TResult>(factoryFunc));
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1>(this IUnityContainer container, Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return container.RegisterType<TResult>(new InjectionParameterizedFactory<TParam1, TResult>(factoryFunc, resolvedParameters));
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2>(this IUnityContainer container, Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return container.RegisterType<TResult>(new InjectionParameterizedFactory<TParam1, TParam2, TResult>(factoryFunc, resolvedParameters));
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2, TParam3>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return container.RegisterType<TResult>(new InjectionParameterizedFactory<TParam1, TParam2, TParam3, TResult>(factoryFunc, resolvedParameters));
        }

        public static IUnityContainer RegisterTypeByFactoryFunc<TResult, TParam1, TParam2, TParam3, TParam4>(this IUnityContainer container, Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return container.RegisterType<TResult>(new InjectionParameterizedFactory<TParam1, TParam2, TParam3, TParam4, TResult>(factoryFunc, resolvedParameters));
        }
    }
}
