using IdeaRS.OpenModel.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionParametrizationExample.Services
{
	/// <summary>
	/// Object store results data to be written to csv file	
	/// </summary>
	public class ConnectionResultInfo
	{
		public List<CheckResSummary> Summary { get; set; }

		public double CalculationTime { get; set; }

		public string ConnectionName { get; set; }

		public List<KeyValuePair<string, object>> CombinationValues { get; set; } 

		public int CombinationIndex { get; set; }

		public double LoadCoefficient { get; set; }

		public int NumberOfIteration { get; set; }

	}

}
