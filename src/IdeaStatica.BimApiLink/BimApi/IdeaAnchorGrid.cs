﻿using IdeaRS.OpenModel.Geometry3D;
using IdeaRS.OpenModel.Parameters;
using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;
using System.Collections.Generic;

namespace IdeaStatica.BimApiLink.BimApi
{
	public class IdeaAnchorGrid : AbstractIdeaObject<IIdeaAnchorGrid>, IIdeaAnchorGrid
	{
		public IdeaAnchorGrid(Identifier<IIdeaAnchorGrid> identifier) : base(identifier)
		{ }

		public IdeaAnchorGrid(string id) : this(new StringIdentifier<IIdeaAnchorGrid>(id))
		{ }

		public AnchorType AnchorType { get; set; }

		public IIdeaConcreteBlock ConcreteBlock { get; set; }

		public double WasherSize { get; set; }

		public virtual IIdeaNode Origin { get; set; }

		public CoordSystem LocalCoordinateSystem { get; set; }

		public IEnumerable<IIdeaNode> Positions { get; set; }

		public IEnumerable<IIdeaObjectConnectable> ConnectedParts { get; set; }

		public bool ShearInThread { get; set; }

		public BoltShearType BoltShearType { get; set; }

		public IIdeaBoltAssembly BoltAssembly { get; set; }

		public IIdeaPersistenceToken Token { get; set; }
	}
}
