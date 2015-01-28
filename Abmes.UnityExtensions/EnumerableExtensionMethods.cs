using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public static class EnumerableExtensionMethods
    {
        public static IUnityContainer RegisterIEnumerable(this IUnityContainer container)
        {
            return container.RegisterType(typeof(IEnumerable<>), new InjectionFactory((c, t, n) => c.ResolveAll(t.GetGenericArguments().Single())));
        }
    }
}
