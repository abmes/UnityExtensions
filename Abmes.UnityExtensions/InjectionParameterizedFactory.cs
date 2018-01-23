using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Injection;
using Unity.Registration;
using Unity.Policy;
using Unity.Builder;

namespace Abmes.UnityExtensions
{
    internal interface IInjectionParameterizedFactory
    {
        Delegate FactoryFunc { get; }
        IEnumerable<ResolvedParameter> ResolvedParameters { get; }
    }

    public class InjectionParameterizedFactory : InjectionMember, IInjectionParameterizedFactory
    {
        private readonly Delegate _factoryFunc;
        private readonly IEnumerable<ResolvedParameter> _resolvedParameters;

        public InjectionParameterizedFactory(Delegate factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            _factoryFunc = factoryFunc;
            _resolvedParameters = resolvedParameters;
        }

        public Delegate FactoryFunc { get { return _factoryFunc; } }
        public IEnumerable<ResolvedParameter> ResolvedParameters { get { return _resolvedParameters; } }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            if (policies == null)
            {
                throw new ArgumentNullException(nameof(policies));
            }

            var policy = new ParameterizedFactoryDelegateBuildPlanPolicy(_factoryFunc, _resolvedParameters.ToArray());
            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        }
    }
}
