using IdeaStatica.BimApiLink.Importers;
using IdeaStatiCa.BimApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaStatica.BimApiLink
{
	public sealed class ImportersConfiguration
	{
		public ImportersConfiguration AddImporter<T>(IImporter<T> importer)
			where T : IIdeaObject
		{

		}	 

		public ImportersConfiguration AddFactory(Func<Type, IImporter> factory)
		{

		}
	}
}
