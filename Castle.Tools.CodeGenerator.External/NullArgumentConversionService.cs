using System.Collections;

namespace Castle.Tools.CodeGenerator.External
{
	public class NullArgumentConversionService : IArgumentConversionService
	{
		public static readonly IArgumentConversionService Instance = new NullArgumentConversionService();
        
		public IDictionary CreateParameters()
		{
			return new Hashtable();
		}

		public bool ConvertArgument(MethodSignature signature, ActionArgument argument, IDictionary parameters)
		{
			return false;
		}
	}
}