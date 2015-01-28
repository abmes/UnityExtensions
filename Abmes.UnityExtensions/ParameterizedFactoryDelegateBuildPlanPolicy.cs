using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    internal class ParameterizedFactoryDelegateBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly Delegate _factoryFunc;
        private readonly IEnumerable<ResolvedParameter> _resolvedParameters;

        public ParameterizedFactoryDelegateBuildPlanPolicy(Delegate factoryFunc, IEnumerable<ResolvedParameter> resolvedParameters)
        {
            _factoryFunc = factoryFunc;
            _resolvedParameters = resolvedParameters;
        }

        private object ResolveParam(Type paramType, IBuilderContext context)
        {
            var resolvedParameter = _resolvedParameters.SingleOrDefault(x => x.ParameterType == paramType);
            return (resolvedParameter == null) ? context.NewBuildUp(new NamedTypeBuildKey(paramType)) : resolvedParameter.GetResolverPolicy(paramType).Resolve(context);
        }

        private IEnumerable<object> ResolveParams(IBuilderContext context)
        {
            return _factoryFunc.Method.GetParameters().Select(x => ResolveParam(x.ParameterType, context));
        }

        public void BuildUp(IBuilderContext context)
        {
            Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(context, "context");

            if (context.Existing == null)
            {
                context.Existing = _factoryFunc.Method.Invoke(_factoryFunc.Target, ResolveParams(context).ToArray());
                DynamicMethodConstructorStrategy.SetPerBuildSingleton(context);
            }
        }
    }
}
