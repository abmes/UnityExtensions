using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    internal class LazyInterceptor<T> : Castle.DynamicProxy.IInterceptor
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

    internal static class LazyProxy
    {
        public static T GetLazyProxy<T>(Func<T> factoryFunc) where T : class
        {
            return new Castle.DynamicProxy.ProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(new LazyInterceptor<T>(factoryFunc));
        }
    }
}
