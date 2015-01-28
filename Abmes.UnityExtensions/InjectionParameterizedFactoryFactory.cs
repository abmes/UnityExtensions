using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public static class InjectionParameterizedFactoryFactory
    {
        public static InjectionParameterizedFactory GetNewInjectionParameterizedFactory(Delegate factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return new InjectionParameterizedFactory(factoryFunc, resolvedParameters);
        }

        public static InjectionParameterizedFactory GetInjectionParameterizedFactory<TResult>(Func<TResult> factoryFunc)
        {
            return GetNewInjectionParameterizedFactory(factoryFunc, new ResolvedParameter[] { });
        }

        public static InjectionParameterizedFactory GetInjectionParameterizedFactory<TParam1, TResult>(Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return GetNewInjectionParameterizedFactory(factoryFunc, resolvedParameters);
        }

        public static InjectionParameterizedFactory GetInjectionParameterizedFactory<TParam1, TParam2, TResult>(Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return GetNewInjectionParameterizedFactory(factoryFunc, resolvedParameters);
        }

        public static InjectionParameterizedFactory GetInjectionParameterizedFactory<TParam1, TParam2, TParam3, TResult>(Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return GetNewInjectionParameterizedFactory(factoryFunc, resolvedParameters);
        }

        public static InjectionParameterizedFactory GetInjectionParameterizedFactory<TParam1, TParam2, TParam3, TParam4, TResult>(Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return GetNewInjectionParameterizedFactory(factoryFunc, resolvedParameters);
        }
    }
}
