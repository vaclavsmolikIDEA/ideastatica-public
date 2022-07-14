using IdeaStatica.BimApiLink.Scope;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Identifiers
{
	internal static class IdentifierExtension
	{
		public static T Instance<T>(this IIdentifier<T> identifier)
			where T : IIdeaObject
		{
			return ScopeVar.Dispatcher.Get(identifier);
		}
	}
}