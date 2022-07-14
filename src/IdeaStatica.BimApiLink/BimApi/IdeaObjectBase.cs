using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.BimApi
{
	public abstract class IdeaObjectBase<T> : IIdeaObject
		where T : IIdeaObject
	{
		public string Id => Identifier.GetStringId();

		public virtual string Name { get; set; } = string.Empty;

		public IIdentifier<T> Identifier { get; }

		protected IdeaObjectBase(IIdentifier<T> identifier)
		{
			Identifier = identifier;
		}
	}
}