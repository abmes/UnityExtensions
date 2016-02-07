using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abmes.UnityExtensions
{
    public static class ConfigureFromSurroundingAssembliesExtensionMethod
    {
        private static bool IsAssemblyConfigurator(Type type) =>
            type.GetInterface(nameof(IUnityContainerConfigurator)) != null;

        private static string CurrentAssemblyPath =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static IEnumerable<string> AllSurroundingAssemblyFileNames =>
            Directory.GetFiles(CurrentAssemblyPath, "*.dll");

        private static IEnumerable<Assembly> AllSurroundingAssemblies =>
            AllSurroundingAssemblyFileNames.Select(Assembly.LoadFile);

        private static IEnumerable<IUnityContainerConfigurator> AllSurroundingConfigurators =>
            AllSurroundingAssemblies
                .SelectMany(x => x.GetTypes().Where(IsAssemblyConfigurator))
                .Select(x => Activator.CreateInstance(x))
                .Cast<IUnityContainerConfigurator>();

        public static void ConfigureFromSurroundingAssemblies(this IUnityContainer container)
        {
            foreach (var configurator in AllSurroundingConfigurators)
            {
                configurator.RegisterTypes(container);
            }
        }
    }
}
