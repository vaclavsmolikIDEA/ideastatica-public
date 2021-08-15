using IdeaStatiCa.BimApi;
using IdeaStatiCa.BimImporter.IntegrationTests.Utils;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaStatiCa.BimImporter.IntegrationTests
{
	[TestFixture]
    public class ConnectionsImportTest
    {
		[Test]
		public void Test()
		{
			string membersJson = File.ReadAllText(@"C:\code\ideastatica-public\src\IdeaStatiCa.BimImporter.IntegrationTests\bin\Debug\net48\members.json");
			var members = JsonConvert.DeserializeObject<List<IIdeaMember1D>>(membersJson, new BimApiJsonConverter());

		}


		private void PrepareModel()
		{

		}
    }
}
