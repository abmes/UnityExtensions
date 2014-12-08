using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.TypedFactories;

namespace Semba.UnityExtensions
{
    public static class AutoFactoryExtensionMethods
    {
        public static IUnityContainer RegisterAutoFactory<TFactoryInterface, TConcreteType>(this IUnityContainer container) where TFactoryInterface : class
        {
            container.RegisterTypedFactory<TFactoryInterface>().ForConcreteType<TConcreteType>();
            return container;
        }

        //public static IUnityContainer RegisterAutoFactory(this IUnityContainer container, Type factoryInterface, Type createdType)
        //{
        //    if (!factoryInterface.IsInterface)
        //    {
        //        throw new ArgumentException("The passed type is not an interface", "factoryInterface");
        //    }

        //    var method = factoryInterface.GetMethods().Single();

        //    //assert createdtype is method.ReturnType

        //    //assert method.parameters == createdType.Constructor.Single.parameters

        //    AssemblyName assemblyName = new AssemblyName("DataBuilderAssembly");
        //    AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        //    var moduleBuilder = assemblyBuilder.DefineDynamicModule("ModuleName");
        //    var factoryTypeBuilder = moduleBuilder.DefineType("TypeName");
        //    factoryTypeBuilder.AddInterfaceImplementation(factoryInterface);
        //    var methodBuilder = factoryTypeBuilder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual, method.ReturnType, method.GetParameters().Select(x => x.ParameterType).ToArray());

        //    var il = methodBuilder.GetILGenerator();

        //    // da map-wa parametrite po ime i tip, a lipswashtite da gi deklarira kato dependency-ta prez konstruktor, koito konteinera shte resolve-ne i da si gi zapomnq
        //    var parameters = method.GetParameters();
        //    for (var i = 0; i < parameters.Count(); i++)
        //    {
        //        il.Emit(OpCodes.Ldarg, i + 1);
        //    }

        //    {
        //        return new createdType(param1, param2, ...);
        //    }
        //    il.Emit(OpCodes.Newobj, createdType.GetConstructors().Single());
        //    il.Emit(OpCodes.Ret);

        //    var factoryType = factoryTypeBuilder.CreateType();

        //    container.RegisterType(factoryInterface, factoryType);

        //    return container;
        //}
    }
}
