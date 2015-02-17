using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public class FactoryFuncBuilder<TResult> where TResult : class
    {
        private readonly IUnityContainer _container;

        public FactoryFuncBuilder(IUnityContainer container)
        {
            _container = container;
        }

        private void CheckDelegateReturnType(Delegate factoryFunc)
        {
            if (typeof(TResult) != factoryFunc.Method.ReturnType)
            {
                throw new Exception(string.Format("Factory function return type \"{0}\" differs from registered type \"{1}\"", factoryFunc.Method.ReturnType, typeof(TResult)));
            }
        }

        public IUnityContainer UsingFunc(Delegate factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            CheckDelegateReturnType(factoryFunc);
            return _container.RegisterType<TResult>(new InjectionParameterizedFactory(factoryFunc, resolvedParameters));
        }

        public IUnityContainer Using(Func<TResult> factoryFunc)
        {
            return UsingFunc(factoryFunc);
        }

        public IUnityContainer UsingLazy(Func<TResult> factoryFunc)
        {
            return Using(() => LazyProxy.GetLazyProxy(() => factoryFunc()));
        }

        public IUnityContainer Using<TParam1>(Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return UsingFunc(factoryFunc);
        }

        public IUnityContainer UsingLazy<TParam1>(Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return Using<Func<TParam1>>((param1Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func())), resolvedParameters);
        }

        public IUnityContainer Using<TParam1, TParam2>(Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return UsingFunc(factoryFunc);
        }

        public IUnityContainer UsingLazy<TParam1, TParam2>(Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return Using<Func<TParam1>, Func<TParam2>>((param1Func, param2Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func())), resolvedParameters);
        }

        public IUnityContainer Using<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return UsingFunc(factoryFunc);
        }

        public IUnityContainer UsingLazy<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return Using<Func<TParam1>, Func<TParam2>, Func<TParam3>>((param1Func, param2Func, param3Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func(), param3Func())), resolvedParameters);
        }

        public IUnityContainer Using<TParam1, TParam2, TParam3, TParam4>(Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return UsingFunc(factoryFunc);
        }
        
        public IUnityContainer UsingLazy<TParam1, TParam2, TParam3, TParam4>(Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return Using<Func<TParam1>, Func<TParam2>, Func<TParam3>, Func<TParam4>>((param1Func, param2Func, param3Func, param4Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func(), param3Func(), param4Func())), resolvedParameters);
        }
    }
}
