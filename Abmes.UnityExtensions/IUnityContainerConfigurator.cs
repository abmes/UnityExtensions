using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public interface IUnityContainerConfigurator
    {
        void RegisterTypes(IUnityContainer container);
    }
}
