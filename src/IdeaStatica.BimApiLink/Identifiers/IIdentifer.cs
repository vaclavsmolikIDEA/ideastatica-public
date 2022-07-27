using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Identifiers
{
	public interface IIdentifier : IIdeaPersistenceToken
	{
		public Type ObjectType { get; }
	}

	public abstract class Identifier<T> : IIdentifier
		where T : IIdeaObject
	{
		public Type ObjectType => typeof(T);

		public abstract string GetStringId();
	}
}