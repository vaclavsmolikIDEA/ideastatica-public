using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Identifiers.Integer
{
	public class Identifier<T> : IIdentifier<T>
		where T : IIdeaObject
	{
		public int Id { get; private set; }

		public Identifier(int id)
		{
			Id = id;
		}

		public string GetStringId()
		{
			return typeof(T).Name + Id.ToString();
		}
	}
}