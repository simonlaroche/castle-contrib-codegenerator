namespace Castle.Tools.CodeGenerator.External
{
	public class NullControllerViewReference : IControllerViewReference
	{
		public static readonly IControllerViewReference Instance = new NullControllerViewReference();

		public void Render(bool skiplayout)
		{
		}

		public void Render()
		{
		}
	}
}