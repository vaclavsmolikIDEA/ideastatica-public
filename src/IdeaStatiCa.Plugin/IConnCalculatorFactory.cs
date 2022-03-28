namespace IdeaStatiCa.Plugin
{
	/// <summary>
	/// Responsible for creating the instance of connection hidden calculator
	/// </summary>
	public interface IConnCalculatorFactory
	{
		/// <summary>
		/// Creates the instance of connection hidden calculator client
		/// </summary>
		/// <exception cref="System.Exception">An exception is thrown if operation fails</exception>
		/// <returns>The instance of the client</returns>
		IConnHiddenCheck Create();
	}
}
