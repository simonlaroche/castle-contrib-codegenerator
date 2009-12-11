using Castle.MonoRail.Framework;

namespace Castle.Tools.CodeGenerator.External
{
	public class NullCodeGeneratorServices : ICodeGeneratorServices
	{
		public static readonly ICodeGeneratorServices Instance = new NullCodeGeneratorServices();

		public Controller Controller { get; set; }
		public IEngineContext RailsContext { get; set; }

		public IControllerReferenceFactory ControllerReferenceFactory
		{
			get { return NullControllerReferenceFactory.Instance; }
		}
		
		public IArgumentConversionService ArgumentConversionService
		{
			get { return NullArgumentConversionService.Instance; }
		}

		public IRuntimeInformationService RuntimeInformationService
		{
			get { return NullRuntimeInformationService.Instance; }
		}
	}
}
