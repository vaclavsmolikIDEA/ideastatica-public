using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionParametrizationExample.Models
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
	public class LoadEffects
	{
		public int id { get; set; }
		public string name { get; set; }
		public bool checkEquilibrium { get; set; }
		public bool percentageLoad { get; set; }
		public List<ForcesOnSegment> forcesOnSegments { get; set; }
		public List<ForcesOnSegment> forcesPercentageOnSegments { get; set; }
	}

	public class ForcesOnSegment
	{
		public int beamSegmentId { get; set; }
		public int position { get; set; }
		public double n { get; set; }
		public double qy { get; set; }
		public double qz { get; set; }
		public double mx { get; set; }
		public double my { get; set; }
		public double mz { get; set; }
		public double absPosition { get; set; }
		public int forceIn { get; set; }
	}

	//public class ForcesPercentageOnSegment
	//{
	//	public int beamSegmentId { get; set; }
	//	public int position { get; set; }
	//	public double n { get; set; }
	//	public double qy { get; set; }
	//	public double qz { get; set; }
	//	public double mx { get; set; }
	//	public double my { get; set; }
	//	public double mz { get; set; }
	//	public double absPosition { get; set; }
	//	public int forceIn { get; set; }
	//}
}
