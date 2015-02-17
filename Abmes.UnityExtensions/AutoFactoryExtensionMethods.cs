using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.TypedFactories;

namespace Abmes.UnityExtensions
{
    public static class AutoFactoryExtensionMethods
    {
        public static IUnityContainer RegisterAutoFactory<TFactoryInterface, TConcreteType>(this IUnityContainer container) where TFactoryInterface : class
        {
            return container.RegisterTypeEx<TFactoryInterface>().AsAutoFactoryFor<TConcreteType>();
        }
    }
}
