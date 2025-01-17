﻿using IdeaStatica.BimApiLink.BimApi;
using IdeaStatica.BimApiLink.Importers;
using IdeaStatiCa.BimApi;

namespace BimApiLinkFeaExample.Importers
{
	internal class MaterialImporter : IntIdentifierImporter<IIdeaMaterial>
	{
		public override IIdeaMaterial Create(int id)
		{
			return new IdeaMaterialByName(id)
			{
				MaterialType = MaterialType.Steel,
				Name = "S 355",
			};
		}
	}
}