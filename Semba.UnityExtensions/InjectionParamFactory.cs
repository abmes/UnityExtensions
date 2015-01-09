using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
{
    internal interface IInjectionParamFactory
    {
        void AddResolvedParameter(ResolvedParameter resolvedParameter);
    }

    public class InjectionParamFactory : InjectionMember, IInjectionParamFactory
    {
        private readonly Delegate _factoryFunc;
        private IEnumerable<ResolvedParameter> _resolvedParameters;

        public InjectionParamFactory(Delegate factoryFunc, params ResolvedParameter[] resolvedParameters)
        {
            _factoryFunc = factoryFunc;
            _resolvedParameters = resolvedParameters;
        }

        public void AddResolvedParameter(ResolvedParameter resolvedParameter)
        {
            _resolvedParameters = _resolvedParameters.Concat(new[] { resolvedParameter });
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(implementationType, "implementationType");
            Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(policies, "policies");

            var policy = new ParamFactoryDelegateBuildPlanPolicy(_factoryFunc, _resolvedParameters.ToArray());
            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        }
    }

    public class InjectionParamFactory<TResult> : InjectionParamFactory
    {
        public InjectionParamFactory(Func<TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }

    public class InjectionParamFactory<TParam1, TResult> : InjectionParamFactory
    {
        public InjectionParamFactory(Func<TParam1, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }

    public class InjectionParamFactory<TParam1, TParam2, TResult> : InjectionParamFactory
    {
        public InjectionParamFactory(Func<TParam1, TParam2, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }

    public class InjectionParamFactory<TParam1, TParam2, TParam3, TResult> : InjectionParamFactory
    {
        public InjectionParamFactory(Func<TParam1, TParam2, TParam3, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }

    public class InjectionParamFactory<TParam1, TParam2, TParam3, TParam4, TResult> : InjectionParamFactory
    {
        public InjectionParamFactory(Func<TParam1, TParam2, TParam3, TParam4, TResult> factoryFunc, params ResolvedParameter[] resolvedParameters)
            : base(factoryFunc, resolvedParameters)
        {
        }
    }
}
