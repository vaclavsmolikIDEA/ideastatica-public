using IdeaRS.OpenModel.CrossSection;
using IdeaRS.OpenModel.Geometry2D;
using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.BimApi
{
	public abstract class IdeaCrossSectionByCenterLine : IdeaObjectBase<IIdeaCrossSectionByCenterLine>, IIdeaCrossSectionByCenterLine
	{
		public virtual IIdeaMaterial Material { get; set; } = null!;

		public virtual CrossSectionType Type { get; set; }

		public virtual PolyLine2D CenterLine { get; set; } = new PolyLine2D();

		public virtual double Radius { get; set; }

		public virtual double Thickness { get; set; }
		public virtual double Rotation { get; set; }

		protected IdeaCrossSectionByCenterLine(IIdentifier<IIdeaCrossSectionByCenterLine> identifier)
			: base(identifier)
		{
		}
	}
	public abstract class IdeaCrossSectionByCenterLine : IdeaObjectBase<IIdeaCrossSectionByCenterLine>, IIdeaCrossSectionByCenterLine
	{
		public virtual IIdeaMaterial Material { get; set; } = null!;

		public virtual CrossSectionType Type { get; set; }

		public virtual PolyLine2D CenterLine { get; set; } = new PolyLine2D();

		public virtual double Radius { get; set; }

		public virtual double Thickness { get; set; }
		public virtual double Rotation { get; set; }

		protected IdeaCrossSectionByCenterLine(IIdentifier<IIdeaCrossSectionByCenterLine> identifier)
			: base(identifier)
		{
		}
	}
}