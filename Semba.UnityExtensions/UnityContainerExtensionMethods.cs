using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.Unity
{
    public static class UnityContainerExtensionMethods
    {
        public static IUnityContainer RegisterXDebugType<TFrom, TTo>(this IUnityContainer container) where TTo : TFrom
        {
#if XDEBUG
            return container.RegisterType<TFrom, TTo>();
#else
            return container;
#endif
        }

        public static IUnityContainer RegisterTypeSingleton<TFrom, TTo>(this IUnityContainer container, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            return container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterTypeSingleton<T>(this IUnityContainer container, params InjectionMember[] injectionMembers)
        {
            return container.RegisterType<T>(new ContainerControlledLifetimeManager(), injectionMembers);
        }
    }
}