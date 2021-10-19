using IdeaRS.OpenModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace IdeaStatiCa.Plugin
{
	public static class IomSerializerJson
	{
		/// <summary>
		/// Creates the instance of <see cref="IdeaRS.OpenModel.OpenModel"/> from json string
		/// </summary>
		/// <param name="jsonString">The input string</param>
		/// <returns>The new instance of OpenModel</returns>
		public static OpenModel OpenModelFromJson(string jsonString)
		{
			var openModel = JsonConvert.DeserializeObject<OpenModel>(jsonString, GetSerializerSettings());
			openModel.ReferenceElementsReconstruction();
			return openModel;
		}

		/// <summary>
		/// Serialize the instance <paramref name="src"/> to json
		/// </summary>
		/// <param name="src">The instance of OpenModel</param>
		/// <returns>Json string</returns>
		public static string ToJson(this OpenModel src)
		{
			return JsonConvert.SerializeObject(src, typeof(OpenModel), GetSerializerSettings());
		}

		private static JsonSerializerSettings GetSerializerSettings()
		{
			var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), Culture = CultureInfo.InvariantCulture, TypeNameHandling = TypeNameHandling.Auto };

			return settings;
		}
	}
}
