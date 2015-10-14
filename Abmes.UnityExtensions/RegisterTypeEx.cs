using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abmes.Unity.TypedFactories;

namespace Abmes.UnityExtensions
{
    public class RegisterTypeEx<TResult> where TResult : class
    {
        private readonly IUnityContainer _container;

        public RegisterTypeEx(IUnityContainer container)
        {
            _container = container;
        }

        // ByFactoryFunc methods

        private void CheckDelegateReturnType(Delegate factoryFunc)
        {
            if (typeof(TResult) != factoryFunc.Method.ReturnType)
            {
                throw new Exception(string.Format("Factory function return type \"{0}\" differs from registered type \"{1}\"", factoryFunc.Method.ReturnType, typeof(TResult)));
            }
        }

        public IUnityContainer ByFactoryDelegate(Delegate factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            CheckDelegateReturnType(factoryFunc);
            return _container.RegisterType<TResult>(new InjectionParameterizedFactory(factoryFunc, resolvedParameters));
        }

        public IUnityContainer ByFactoryFunc(Func<TResult> factoryFunc)
        {
            return ByFactoryDelegate(factoryFunc);
        }

        public IUnityContainer ByFactoryFuncLazy(Func<TResult> factoryFunc)
        {
            return ByFactoryFunc(() => LazyProxy.GetLazyProxy(() => factoryFunc()));
        }

        public IUnityContainer ByFactoryFunc<TParam1>(Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryDelegate(factoryFunc);
        }

        public IUnityContainer ByFactoryFuncLazy<TParam1>(Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryFunc<Func<TParam1>>((param1Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func())), resolvedParameters);
        }

        public IUnityContainer ByFactoryFunc<TParam1, TParam2>(Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryDelegate(factoryFunc);
        }

        public IUnityContainer ByFactoryFuncLazy<TParam1, TParam2>(Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryFunc<Func<TParam1>, Func<TParam2>>((param1Func, param2Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func())), resolvedParameters);
        }

        public IUnityContainer ByFactoryFunc<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryDelegate(factoryFunc);
        }

        public IUnityContainer ByFactoryFuncLazy<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryFunc<Func<TParam1>, Func<TParam2>, Func<TParam3>>((param1Func, param2Func, param3Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func(), param3Func())), resolvedParameters);
        }

        public IUnityContainer ByFactoryFunc<TParam1, TParam2, TParam3, TParam4>(Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryDelegate(factoryFunc);
        }
        
        public IUnityContainer ByFactoryFuncLazy<TParam1, TParam2, TParam3, TParam4>(Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            return ByFactoryFunc<Func<TParam1>, Func<TParam2>, Func<TParam3>, Func<TParam4>>((param1Func, param2Func, param3Func, param4Func) => LazyProxy.GetLazyProxy(() => factoryFunc(param1Func(), param2Func(), param3Func(), param4Func())), resolvedParameters);
        }

        // AsAutoFactory method

        public IUnityContainer AsAutoFactoryFor<TConcreteType>()
        {
            _container.RegisterTypedFactory<TResult>().ForConcreteType<TConcreteType>();
            return _container;
        }
    }
}
