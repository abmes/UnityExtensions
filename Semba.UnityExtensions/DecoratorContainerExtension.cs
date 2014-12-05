using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
{
    public class DecoratorContainerExtension : UnityContainerExtension
    {
        private IDictionary<Tuple<Type, string>, IEnumerable<Type>> _decoratorChains;

        protected override void Initialize()
        {
            _decoratorChains = new Dictionary<Tuple<Type, string>, IEnumerable<Type>>();
            Context.Registering += AddRegistration;
            Context.Strategies.Add(new DecoratorBuildStrategy(_decoratorChains), UnityBuildStage.PreCreation);
        }

        private void AddRegistration(object sender, RegisterEventArgs e)
        {
            if (e.TypeFrom.IsInterface)
            {
                var key = Tuple.Create(e.TypeFrom, e.Name);
                if (!_decoratorChains.ContainsKey(key))
                {
                    _decoratorChains.Add(key, new[] { e.TypeTo });
                }
                else
                {
                    _decoratorChains[key] = _decoratorChains[key].Concat(new[] { e.TypeTo });
                }
            }
        }
    }
}
