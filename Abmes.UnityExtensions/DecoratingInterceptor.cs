using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    class DecoratingInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private const string DecoratorNamePattern = "Decorator Type {0} No {1}";
        private IDictionary<Type, int> _decoratorCounters;
        private IUnityContainer _container;

        public DecoratingInterceptor(IUnityContainer container)
        {
            _container = container;
            _decoratorCounters = new Dictionary<Type, int>();
        }

        private void EnsureDecoratorCounterExists(Type t)
        {
            if (!_decoratorCounters.ContainsKey(t))
            {
                _decoratorCounters[t] = 0;
            }
        }

        private int GetCurrentDecoratorNo(Type t)
        {
            EnsureDecoratorCounterExists(t);
            return _decoratorCounters[t];
        }

        private void IncreaseDecoratorCount(Type t)
        {
            EnsureDecoratorCounterExists(t);
            _decoratorCounters[t] = _decoratorCounters[t] + 1;
        }

        private string GetNewDecoratorName(Type t)
        {
            IncreaseDecoratorCount(t);
            return string.Format(DecoratorNamePattern, t.Name, GetCurrentDecoratorNo(t));
        }

        private string PeekNextDecoratorName(Type t)
        {
            return string.Format(DecoratorNamePattern, t.Name, GetCurrentDecoratorNo(t) + 1);
        }
        
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            if ((invocation.Method.Name == "RegisterType") && (invocation.Arguments[0] == null))
            {
                invocation.Arguments[0] = invocation.Arguments[1];
            }

            if (invocation.Method.Name == "RegisterType")
            {
                var t = (Type)invocation.Arguments[0];
                var concreteType = (Type)invocation.Arguments[1];
                var name = (string)invocation.Arguments[2];
                var injectionMembers = (InjectionMember[])invocation.Arguments[4];

                Func<Type, bool> typeIsSame = type => (type == t) || (type == typeof(Func<>).MakeGenericType(t)) || (type == typeof(Lazy<>).MakeGenericType(t));

                if (string.IsNullOrEmpty(name))
                {
                    name = null;
                }

                if (name == null)
                {
                    name = _container.Registrations.Any(x => x.RegisteredType == t) ? GetNewDecoratorName(t) : null;

                    if (concreteType.IsClass)
                    {
                        var constructor = concreteType.GetConstructors().SingleOrDefault();

                        if ((constructor != null) && (constructor.GetParameters().Any(p => typeIsSame(p.ParameterType))))
                        {
                            var resolvedParameters =
                                constructor.GetParameters()
                                .Select(x => new ResolvedParameter(x.ParameterType, typeIsSame(x.ParameterType) ? PeekNextDecoratorName(t) : null))
                                .ToArray();

                            // star injectionconstructor trqbwa da se podmenq s now , koito da e sys syshtite ResolvedParams, samo za t(bez ime ) da za nowi
                            // za sega kazwame che ne se poddyrja
                            if (injectionMembers.OfType<InjectionConstructor>().Any())
                            {
                                throw new Exception("InjectionContructor cannot be used with decoration");
                            }

                            injectionMembers = injectionMembers.Concat(new[] { new InjectionConstructor(resolvedParameters) }).ToArray();
                        }
                    }

                    if (concreteType.IsInterface)
                    {
                        injectionMembers =
                            injectionMembers.Select(im =>
                            {
                                var factory = im as IInjectionParameterizedFactory;

                                if ((factory != null) && factory.FactoryFunc.Method.GetParameters().Any(p => typeIsSame(p.ParameterType) && !factory.ResolvedParameters.Any(rp => typeIsSame(rp.ParameterType))))
                                {
                                    return InjectionParameterizedFactoryFactory.GetNewInjectionParameterizedFactory(factory.FactoryFunc, factory.ResolvedParameters.Concat(new[] { new ResolvedParameter(t, PeekNextDecoratorName(t)) }).ToArray());
                                }
                                else
                                {
                                    return im;
                                }
                            }).ToArray();
                    }

                    invocation.Arguments[2] = name;
                    invocation.Arguments[4] = injectionMembers;
                }
            }

            var result = invocation.Method.Invoke(_container, invocation.Arguments);

            invocation.ReturnValue = (result == _container) ? invocation.Proxy : result;
        }
    }
}
