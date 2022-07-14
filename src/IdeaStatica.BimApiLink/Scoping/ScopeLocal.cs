using IdeaStatica.BimApiLink.Importers;

namespace IdeaStatica.BimApiLink.Scope
{
	internal class ScopeVar
	{
		public static ImporterDispatcher Dispatcher
			=> Scope.Current.Get<ImporterDispatcher>("importerDispatcher");
	}
}