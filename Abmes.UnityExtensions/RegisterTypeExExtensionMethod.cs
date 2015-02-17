using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public static class RegisterTypeExExtensionMethod
    {
        public static RegisterTypeEx<TResult> RegisterTypeEx<TResult>(this IUnityContainer container) where TResult : class
        {
            return new RegisterTypeEx<TResult>(container);
        }
    }
}
