using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
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
            Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(implementationType, "implementationType");
            Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(policies, "policies");

            var policy = new ParameterizedFactoryDelegateBuildPlanPolicy(_factoryFunc, _resolvedParameters.ToArray());
            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        }
    }
}
