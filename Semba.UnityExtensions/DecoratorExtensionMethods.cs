using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semba.UnityExtensions
{
    public static class DecoratorExtensionMethod
    {
        public static IUnityContainer EnableDecoration(this IUnityContainer container)
        {
            return container.AddExtension(new DecoratorContainerExtension());
        }
    }
}
