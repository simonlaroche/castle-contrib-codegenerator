using System;

namespace Castle.Tools.CodeGenerator.External
{
	public class NullRuntimeInformationService : IRuntimeInformationService
	{
		public static readonly IRuntimeInformationService Instance = new NullRuntimeInformationService();

		public MethodInformation ResolveMethodInformation(MethodSignature signature)
		{
			return null;
		}

		public MethodInformation ResolveMethodInformation(Type type, string name, Type[] types)
		{
			return null;
		}
	}
}