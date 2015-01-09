using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
{
    public static class DecoratorExtensionMethods
    {
        private static IUnityContainer GetDecoratingProxy(IUnityContainer container)
        {
            var proxyGenerator = new Castle.DynamicProxy.ProxyGenerator();
            return proxyGenerator.CreateInterfaceProxyWithoutTarget<IUnityContainer>(new DecoratingInterceptor(container));
        }

        public static IUnityContainer EnableDecoration(this IUnityContainer container)
        {
            return GetDecoratingProxy(container);
        }
    }
}
