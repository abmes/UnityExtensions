using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Semba.UnityExtensions
{
    public static class UnityContainerExtensionMethods
    {
        public static IUnityContainer RegisterTypeSingleton<TFrom, TTo>(this IUnityContainer container, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            return container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterTypeSingleton<T>(this IUnityContainer container, params InjectionMember[] injectionMembers)
        {
            return container.RegisterType<T>(new ContainerControlledLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterIEnumerable(this IUnityContainer container)
        {
            return container.RegisterType(typeof(IEnumerable<>), new InjectionFactory((c, t, n) => c.ResolveAll(t.GetGenericArguments().Single())));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom>(this IUnityContainer container, params Type[] decoratorChain)
        {
            string previousRegistrationName = null;

            foreach (var t in decoratorChain)
            {
                var currentRegistrationName = (t == decoratorChain.Last()) ? "" : Guid.NewGuid().ToString();
                var constructor = t.GetConstructors().Single();
                var parameters = constructor.GetParameters().Select(x => x.ParameterType == typeof(TFrom) ? new ResolvedParameter<TFrom>(previousRegistrationName) : new ResolvedParameter(x.ParameterType)).ToArray();
                container.RegisterType(typeof(TFrom), t, currentRegistrationName, new InjectionConstructor(parameters));
                previousRegistrationName = currentRegistrationName;
            }

            return container;
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator>(this IUnityContainer container)
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2>(this IUnityContainer container)
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator1), typeof(TDecorator2));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2, TDecorator3>(this IUnityContainer container)
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator1), typeof(TDecorator2), typeof(TDecorator3));
        }
    }
}