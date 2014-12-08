using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
{
    static class DecoratorChainExtensionMethods
    {
        public static IUnityContainer RegisterDecoratorChain<TFrom>(this IUnityContainer container, LifetimeManager lifetimeManager, params Type[] decoratorChain)
        {
            string previousRegistrationName = null;

            foreach (var t in decoratorChain)
            {
                var currentRegistrationName = (t == decoratorChain.Last()) ? "" : Guid.NewGuid().ToString();
                var currentLifetimeManager = (t == decoratorChain.Last()) ? lifetimeManager : new TransientLifetimeManager();
                var constructor = t.GetConstructors().Single();
                var parameters = constructor.GetParameters().Select(x => x.ParameterType == typeof(TFrom) ? new ResolvedParameter<TFrom>(previousRegistrationName) : new ResolvedParameter(x.ParameterType)).ToArray();
                container.RegisterType(typeof(TFrom), t, currentRegistrationName, currentLifetimeManager, new InjectionConstructor(parameters));
                previousRegistrationName = currentRegistrationName;
            }

            return container;
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom>(this IUnityContainer container, params Type[] decoratorChain)
        {
            return container.RegisterDecoratorChain<TFrom>(new TransientLifetimeManager(), decoratorChain);
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator>(this IUnityContainer container, LifetimeManager lifetimeManager)
            where TTo : TFrom
            where TDecorator : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(lifetimeManager, typeof(TTo), typeof(TDecorator));
        }

        public static IUnityContainer RegisterSingletonDecoratorChain<TFrom, TTo, TDecorator>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(new ContainerControlledLifetimeManager(), typeof(TTo), typeof(TDecorator));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator1), typeof(TDecorator2));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2>(this IUnityContainer container, LifetimeManager lifetimeManager)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(lifetimeManager, typeof(TTo), typeof(TDecorator1), typeof(TDecorator2));
        }

        public static IUnityContainer RegisterSingletonDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(new ContainerControlledLifetimeManager(), typeof(TTo), typeof(TDecorator1), typeof(TDecorator2));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2, TDecorator3>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
            where TDecorator3 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(typeof(TTo), typeof(TDecorator1), typeof(TDecorator2), typeof(TDecorator3));
        }

        public static IUnityContainer RegisterDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2, TDecorator3>(this IUnityContainer container, LifetimeManager lifetimeManager)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
            where TDecorator3 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(lifetimeManager, typeof(TTo), typeof(TDecorator1), typeof(TDecorator2), typeof(TDecorator3));
        }

        public static IUnityContainer RegisterSingletonDecoratorChain<TFrom, TTo, TDecorator1, TDecorator2, TDecorator3>(this IUnityContainer container)
            where TTo : TFrom
            where TDecorator1 : TFrom
            where TDecorator2 : TFrom
            where TDecorator3 : TFrom
        {
            return container.RegisterDecoratorChain<TFrom>(new ContainerControlledLifetimeManager(), typeof(TTo), typeof(TDecorator1), typeof(TDecorator2), typeof(TDecorator3));
        }
    }
}
