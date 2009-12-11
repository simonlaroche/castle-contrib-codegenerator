using System;

namespace Castle.Tools.CodeGenerator.External
{
	public class NullControllerReferenceFactory : IControllerReferenceFactory
	{
		public static readonly IControllerReferenceFactory Instance = new NullControllerReferenceFactory();

		public IControllerActionReference CreateActionReference(ICodeGeneratorServices services, Type controllerType, string area, string controller, string action, MethodSignature signature, ActionArgument[] arguments)
		{
			return NullControllerActionReference.Instance;
		}

		public IControllerViewReference CreateViewReference(ICodeGeneratorServices services, Type controllerType, string area, string controller, string action)
		{
			return NullControllerViewReference.Instance;
		}
	}
}