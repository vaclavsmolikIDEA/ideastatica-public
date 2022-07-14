using IdeaStatica.BimApiLink.Persistence;
using IdeaStatiCa.BimImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaStatica.BimApiLink
{
	public abstract class BimApiApplication : ProjectApplication
	{
		protected BimApiApplication(IProject project, IProjectStorage projectStorage) 
			: base(project, projectStorage)
		{
		}
	}
}
