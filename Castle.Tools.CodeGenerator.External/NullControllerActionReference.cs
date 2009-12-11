namespace Castle.Tools.CodeGenerator.External
{
	public class NullControllerActionReference : IControllerActionReference
	{
		public static readonly IControllerActionReference Instance = new NullControllerActionReference();

		public string Url
		{
			get { return ""; }
		}

		public void Redirect(bool useJavascript)
		{
		}

		public void Redirect()
		{
		}

		public void Transfer()
		{
		}
	}
}