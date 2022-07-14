using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.BimApi
{
	public class IdeaNode : IdeaObjectBase<IIdeaNode>, IIdeaNode
	{
		public virtual IdeaVector3D Vector { get; set; } = null!;

		public virtual IIdeaPersistenceToken Token { get; set; }

		protected IdeaNode(IIdentifier<IIdeaNode> identifer)
			: base(identifer)
		{
			Token = identifer;
		}

		public sealed class Ref : IdeaNode
		{
			public override string Name => Identifier.Instance().Name;

			public override IIdeaPersistenceToken Token => Identifier.Instance().Token;

			public override IdeaVector3D Vector => Identifier.Instance().Vector;

			public Ref(IIdentifier<IIdeaNode> identifer) : base(identifer)
			{
			}
		}
	}
}