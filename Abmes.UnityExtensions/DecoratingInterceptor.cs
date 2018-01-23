using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Registration;
using Unity.Injection;

namespace Abmes.UnityExtensions
{
    class DecoratingInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private const string DecoratorNamePattern = "Decorator Type {0} Name {1} No {2}";
        private IDictionary<Tuple<Type, string>, int> _decoratorCounters;
        private IUnityContainer _container;

        public DecoratingInterceptor(IUnityContainer container)
        {
            _container = container;
            _decoratorCounters = new Dictionary<Tuple<Type, string>, int>();
        }

        private void EnsureDecoratorCounterExists(Tuple<Type, string> key)
        {
            if (!_decoratorCounters.ContainsKey(key))
            {
                _decoratorCounters[key] = 0;
            }
        }

        private int GetCurrentDecoratorNo(Tuple<Type, string> key)
        {
            EnsureDecoratorCounterExists(key);
            return _decoratorCounters[key];
        }

        private void IncreaseDecoratorCount(Tuple<Type, string> key)
        {
            EnsureDecoratorCounterExists(key);
            _decoratorCounters[key] = _decoratorCounters[key] + 1;
        }

        private string GetNewDecoratorName(Type t, string name)
        {
            IncreaseDecoratorCount(Tuple.Create(t, name));
            return string.Format(DecoratorNamePattern, t.Name, name, GetCurrentDecoratorNo(Tuple.Create(t, name)));
        }

        private string PeekNextDecoratorName(Type t, string name)
        {
            return string.Format(DecoratorNamePattern, t.Name, name, GetCurrentDecoratorNo(Tuple.Create(t, name)) + 1);
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

                name = (name == "") ? null : name;

                var newName = _container.Registrations.Any(x => (x.RegisteredType == t) && (x.Name == name)) ? GetNewDecoratorName(t, name) : name;

                if (concreteType.IsClass)
                {
                    var constructors = concreteType.GetConstructors();
                    if (constructors.Length == 1)
                    {
                        var constructor = constructors.Single();

                        if (constructor.GetParameters().Any(p => typeIsSame(p.ParameterType)))
                        {
                            var resolvedParameters =
                                constructor.GetParameters()
                                .Select(x => new ResolvedParameter(x.ParameterType, typeIsSame(x.ParameterType) ? PeekNextDecoratorName(t, name) : null))
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
                }

                if (concreteType.IsInterface)
                {
                    injectionMembers =
                        injectionMembers.Select(im =>
                        {
                            var factory = im as IInjectionParameterizedFactory;

                            if ((factory != null) && factory.FactoryFunc.Method.GetParameters().Any(p => typeIsSame(p.ParameterType) && !factory.ResolvedParameters.Any(rp => typeIsSame(rp.ParameterType))))
                            {
                                return new InjectionParameterizedFactory(factory.FactoryFunc, factory.ResolvedParameters.Concat(new[] { new ResolvedParameter(t, PeekNextDecoratorName(t, name)) }).ToArray());
                            }
                            else
                            {
                                return im;
                            }
                        }).ToArray();
                }

                invocation.Arguments[2] = newName;
                invocation.Arguments[4] = injectionMembers;
            }

            var result = invocation.Method.Invoke(_container, invocation.Arguments);

            invocation.ReturnValue = (result == _container) ? invocation.Proxy : result;
        }
    }
}
