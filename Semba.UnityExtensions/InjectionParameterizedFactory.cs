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
        //IEnumerable<ResolvedParameter> ResolvedParameters { get; }
        void AddResolvedParameter(ResolvedParameter resolvedParameter);

    }

    public class InjectionParameterizedFactory : InjectionMember, IInjectionParameterizedFactory
    {
        private readonly Delegate _factoryFunc;
        private IEnumerable<ResolvedParameter> _resolvedParameters;

        public InjectionParameterizedFactory(Delegate factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            _factoryFunc = factoryFunc;
            _resolvedParameters = resolvedParameters;
        }

        //public IEnumerable<ResolvedParameter> ResolvedParameters { get { return _resolvedParameters; } }
        public void AddResolvedParameter(ResolvedParameter resolvedParameter)
        {
            _resolvedParameters = _resolvedParameters.Concat(new[] { resolvedParameter });
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(implementationType, "implementationType");
            Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(policies, "policies");

            var policy = new ParameterizedFactoryDelegateBuildPlanPolicy(_factoryFunc, _resolvedParameters.ToArray());
            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        }
    }

    public class InjectionParameterizedFactory<TResult> : InjectionParameterizedFactory
    {
        public InjectionParameterizedFactory(Func<TResult> factoryFunc)
            : base(factoryFunc)
        {
        }
    }

    public class InjectionParameterizedFactory<TParam1, TResult> : InjectionParameterizedFactory
    {
        public InjectionParameterizedFactory(Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }

    public class InjectionParameterizedFactory<TParam1, TParam2, TResult> : InjectionParameterizedFactory
    {
        public InjectionParameterizedFactory(Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }

    public class InjectionParameterizedFactory<TParam1, TParam2, TParam3, TResult> : InjectionParameterizedFactory
    {
        public InjectionParameterizedFactory(Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }

    public class InjectionParameterizedFactory<TParam1, TParam2, TParam3, TParam4, TResult> : InjectionParameterizedFactory
    {
        public InjectionParameterizedFactory(Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }
}
