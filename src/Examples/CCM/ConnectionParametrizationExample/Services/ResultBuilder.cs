using IdeaRS.OpenModel.Connection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConnectionParametrizationExample.Services
{
	/// <summary>
	/// Store and build results
	/// </summary>
	public class ResultBuilder
	{
		Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
		string resultSeperator;

		public ResultBuilder(string resultSeperator = ";")
		{
			results = new Dictionary<string, List<string>>();
			this.resultSeperator = resultSeperator;
		}

		public void AddResult(string key, double calculationTime, List<CheckResSummary> resultSummary, List<KeyValuePair<string, object>> combination)
		{
			if (!results.ContainsKey(key))
			{
				// Create new list for a key
				results[key] = new List<string>();

				// Add headers
				List<string> headers = new List<string>();
				headers.Add("Time [s]");
				headers.AddRange(resultSummary.Select(x => x.Name));
				headers.AddRange(combination.Select(y => y.Key));
				results[key].Add(string.Join(resultSeperator, headers));
			}

			// Add values
			List<string> resultValues = new List<string>();
			resultValues.Add(calculationTime.ToString());
			resultValues.AddRange(resultSummary.Select(x => x.CheckValue.ToString()));
			resultValues.AddRange(combination.Select(y => y.Value.ToString()));
			results[key].Add(string.Join(resultSeperator, resultValues));
		}

		public void WriteAllResultsToCsv(string path)
		{
			foreach(KeyValuePair<string, List<string>> result in results)
			{
				string resultFilePath = Path.Combine(path, $"{result.Key}.csv");
				File.WriteAllLines(resultFilePath, result.Value);
			}
		}
	}
}
