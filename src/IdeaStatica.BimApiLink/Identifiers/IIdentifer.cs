using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Identifiers
{
	public interface IIdentifier : IIdeaPersistenceToken
	{ }

	public interface IIdentifier<T> : IIdentifier
		where T : IIdeaObject
	{
		string GetStringId();
	}
}