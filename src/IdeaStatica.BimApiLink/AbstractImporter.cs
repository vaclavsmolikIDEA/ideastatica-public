using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaStatica.BimApiLink
{
	public abstract class AbstractImporter<T>
		where T: IIdeaObject
	{
		public abstract T Create(IIdentifier<T> identifer);
	}
}
