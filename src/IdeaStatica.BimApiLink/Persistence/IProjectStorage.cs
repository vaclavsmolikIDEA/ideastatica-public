using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaStatica.BimApiLink.Persistence
{
	public interface IProjectStorage
	{
		void Load();

		void Save();
	}
}
