using IdeaStatica.BimApiLink.BimApi;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Identifiers.Integer
{
	public static class IdeaObjectBaseExtension
	{
		public static int Id<T>(this IdeaObjectBase<T> ideaObject)
			where T: IIdeaObject
		{
			if (ideaObject.Identifier is Identifier<T> identifier)
			{
				return identifier.Id;
			}

			throw new ArgumentException();
		}
	}
}