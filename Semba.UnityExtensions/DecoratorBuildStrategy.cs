using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
{
    public class DecoratorBuildStrategy : BuilderStrategy
    {
        private readonly IDictionary<Tuple<Type, string>, IEnumerable<Type>> _decoratorChains;

        public DecoratorBuildStrategy(IDictionary<Tuple<Type, string>, IEnumerable<Type>> decoratorChains)
        {
            _decoratorChains = decoratorChains;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            var buildKey = context.OriginalBuildKey;

            var dictType = buildKey.Type.IsGenericType ? buildKey.Type.GetGenericTypeDefinition() : buildKey.Type;
            var dictKey = Tuple.Create(dictType, buildKey.Name);

            if (buildKey.Type.IsInterface && _decoratorChains.ContainsKey(dictKey) && (context.GetOverriddenResolver(buildKey.Type) == null))
            {
                object value = null;
                foreach (var type in _decoratorChains[dictKey].Reverse())
                {
                    var actualTypeToBuild = type.IsGenericTypeDefinition ? type.MakeGenericType(buildKey.Type.GetGenericArguments()) : type;
                    value = context.NewBuildUp(new NamedTypeBuildKey(actualTypeToBuild, buildKey.Name));
                    context.AddResolverOverrides(new DependencyOverride(buildKey.Type, value));
                }

                context.Existing = value;
                context.BuildComplete = true;
            }
        }
    }
}
