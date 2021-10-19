using IdeaStatiCa.Plugin;
using IOM.GeneratorExample;
using Xunit;
using FluentAssertions;

namespace UT_OpenModel
{
	public class OpenModelSerialization
	{
		[Fact]
		public void OpenModelJsonTest()
		{
			var source = SteelFrameExample.CreateIOM();
			var json = source.ToJson();

			var clone = IomSerializerJson.OpenModelFromJson(json);
			//clone.Should().BeEquivalentTo(source);
		}
	}
}
