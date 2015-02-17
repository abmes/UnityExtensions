using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public static class FactoryFuncExtensionMethods
    {
        public static FactoryFuncBuilder<TResult> RegisterTypeByFactoryFunc<TResult>(this IUnityContainer container) where TResult : class
        {
            return new FactoryFuncBuilder<TResult>(container);
        }
    }
}
