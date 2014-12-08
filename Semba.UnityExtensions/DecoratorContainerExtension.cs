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
            Context.Strategies.Add(new DecoratorBuildStrategy(_decoratorChains), UnityBuildStage.PreCreation);
            Context.Registering += AddRegistration;
        }

        public override void Remove()
        {
            Context.Registering -= AddRegistration;
            _decoratorChains.Clear();
        }

        private void AddRegistration(object sender, RegisterEventArgs e)
        {
            var typeTo = e.TypeTo;
            var typeFrom = e.TypeFrom ?? e.TypeTo;
            
            if (typeFrom.IsInterface)
            {
                var key = Tuple.Create(typeFrom, e.Name);
                if (!_decoratorChains.ContainsKey(key))
                {
                    _decoratorChains.Add(key, new[] { typeTo });
                }
                else
                {
                    _decoratorChains[key] = _decoratorChains[key].Concat(new[] { typeTo });
                }
            }
        }
    }
}
