﻿using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.BimApi
{
	public class IdeaConnectedMember : AbstractIdeaObject<IIdeaConnectedMember>, IIdeaConnectedMember
	{
		public IdeaConnectedMember(Identifier<IIdeaConnectedMember> identifier) : base(identifier)
		{
		}

		public IdeaConnectedMember(string id)
			: this(new StringIdentifier<IIdeaConnectedMember>(id))
		{
		}

		public IdeaGeometricalType GeometricalType { get; set; }

		public IdeaConnectedMemberType ConnectedMemberType { get; set; }

		public bool IsBearing { get; set; }

		public IdeaForcesIn ForcesIn { get; set; }

		public IdeaBeamSegmentModelType MemberSegmentType { get; set; }

		public IIdeaMember1D IdeaMember { get; set; }

		public bool AutoAddCutByWorkplane { get; set; }
	}
}