using Unity;
using Unity.Registration;
using Unity.Lifetime;

namespace Abmes.UnityExtensions
{
    public static class SingletonExtensionMethods
    {
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
