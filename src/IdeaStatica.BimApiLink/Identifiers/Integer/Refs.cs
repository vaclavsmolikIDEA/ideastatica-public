using IdeaStatica.BimApiLink.BimApi;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Identifiers.Integer
{
	public static class Refs
	{
		public static IIdeaNode Node(int id)
		{
			return new IdeaNode.Ref(new Identifier<IIdeaNode>(id));
		}
	}
}