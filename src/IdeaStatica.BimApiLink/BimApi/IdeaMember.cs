using IdeaRS.OpenModel.Model;
using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;
using IdeaStatiCa.BimApi.Results;

namespace IdeaStatica.BimApiLink.BimApi
{
	public class IdeaMember : IdeaObjectBase<IIdeaMember1D>, IIdeaMember1D
	{
		public virtual IIdeaPersistenceToken Token { get; set; }

		public virtual Member1DType Type { get; set; } = Member1DType.Beam;

		public virtual List<IIdeaElement1D> Elements { get; set; } = new List<IIdeaElement1D>();

		public virtual IIdeaTaper Taper { get; set; } = null!;

		public virtual IIdeaCrossSection CrossSection { get; set; } = null!;

		public virtual Alignment Alignment { get; set; } = Alignment.Center;

		protected IdeaMember(IIdentifier<IIdeaMember1D> identifier)
			: base(identifier)
		{
			Token = identifier;
		}

		public virtual IEnumerable<IIdeaResult> GetResults()
		{
			return Enumerable.Empty<IIdeaResult>();
		}

		public class Ref : IdeaMember
		{
			public Ref(IIdentifier<IIdeaMember1D> identifier) : base(identifier)
			{
			}
		}
	}
}