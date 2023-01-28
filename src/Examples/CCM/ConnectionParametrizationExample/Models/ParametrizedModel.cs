using IdeaStatiCa.Plugin;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using ConnectionParametrizationExample.Extensions;
using System.ComponentModel;
using ConnectionParametrizationExample.Services;
using IdeaRS.OpenModel.Connection;
using Newtonsoft.Json.Linq;

namespace ConnectionParametrizationExample.Models
{
	public class ParametrizedModel
	{
		List<string> resultsSummaryItems = new List<string> { "Plates", "Loc. deformation", "Bolts", "Anchors", "Preloaded bolts", "Welds", "Concrete block", "Shear" };
		//List<string> resultsSummaryItems = new List<string> { "Analysis", "Plates", "Loc. deformation", "Bolts", "Anchors", "Preloaded bolts", "Welds", "Concrete block", "Shear", "Buckling" };

		public string IdeaAppLocation { get; set; }
		public string IdeaConFilesLocation { get; set; }
		private ConnectionHiddenCheckClient client;

		public void RunParametrizedAnalysis(List<string> IdeaConFiles, List<string> stopAtLimitResultItems, BackgroundWorker worker)
		{
			// Create result writer
			ResultBuilder resultBuilder = new ResultBuilder();

			// Create the instance of factory - it looks for IDEA StatiCa in the directory 'ideaStaticaInstallDir'
			ConnHiddenClientFactory calcFactory = new ConnHiddenClientFactory(IdeaAppLocation.Replace(@"\\", @"\"));

			// Create the instance of the IDEA StatiCa Client
			client = calcFactory.Create();

			try
			{
				// Load json with parameters
				string codeSetupParametersPath = Path.Combine(IdeaConFilesLocation, "codeSetupParameters.json");
				string codeSetupParameters = File.ReadAllText(codeSetupParametersPath);

				// Get all possible combinations of parameters
				var parameterCombinations = GetParameterCombinations(codeSetupParameters);

				// For each ideaCon model
				foreach (string ideaConFile in IdeaConFiles)
				{
					// Report progress
					double indexOf = IdeaConFiles.FindIndex(x => x == ideaConFile);
					worker.ReportProgress((int)(indexOf / IdeaConFiles.Count * 100));

					// Open project on the service side
					client.OpenProject(ideaConFile);

					try
					{
						// Get connection project info
						var projInfo = client.GetProjectInfo();

						if (projInfo?.Connections != null)
						{
							// Iterate over all combinations
							foreach (var combination in parameterCombinations.Select((x, i) => new { Value = x, Index = i }))
							{
								// Save initial loads
								var initialLoads = GetAllLoadsFromProject(projInfo);

								// Iterate all connections in the project
								foreach (var con in projInfo.Connections)
								{
									// Get initial loads and update code setup
									UpdateCodeSetup(combination.Value);

									// Calculate model
									ConnectionResultInfo result = CalculateUptoMaximumUtilization(client, con, stopAtLimitResultItems);

									// Add info to result
									result.CombinationValues = combination.Value.ToList();
									result.CombinationIndex = combination.Index;
									result.ConnectionName = $"{Path.GetFileNameWithoutExtension(ideaConFile)}_{con.Name}";

									// Add result to builder
									resultBuilder.AddResult(result);
								}
								// Save Project
								string calculatedModels = "CalculatedModels";
								if (!Directory.Exists(Path.Combine(IdeaConFilesLocation, calculatedModels)))
								{
									Directory.CreateDirectory(Path.Combine(IdeaConFilesLocation, calculatedModels));
								}
								string newProjectPath = Path.Combine(IdeaConFilesLocation, calculatedModels, $"{Path.GetFileNameWithoutExtension(ideaConFile)}_{combination.Index}.ideaCon");
								client.SaveAsProject(newProjectPath);

								// Reset load to initial values
								ResetLoads(projInfo, initialLoads);
							}
						}
					}
					finally
					{
						// Delete temps in case of a crash
						client.CloseProject();
					}
				}
			}
			finally
			{
				client?.Close();

				// Write results to csv files
				resultBuilder.WriteAllResultsToCsv(IdeaConFilesLocation);
			}
		}

		private IEnumerable<IEnumerable<KeyValuePair<string, object>>> GetParameterCombinations(string parameters)
		{
			Dictionary<string, List<object>> param = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(parameters);

			// All parameters to list
			List<KeyValuePair<string, object>> parameterList = new List<KeyValuePair<string, object>>();

			foreach (KeyValuePair<string, List<object>> entry in param)
			{
				foreach (var value in entry.Value)
				{
					parameterList.Add(new KeyValuePair<string, object>(entry.Key, value));
				}
			}

			// Get all possible combinations of parameters
			var parameterCombinations = parameterList.GroupBy(t => t.Key).CartesianProduct();

			return parameterCombinations;
		}

		private void UpdateCodeSetup(IEnumerable<KeyValuePair<string, object>> combination)
		{
			// Get code setup
			string codeSetup = client.GetCodeSetupJSON();

			JObject jObject = JsonConvert.DeserializeObject(codeSetup) as JObject;

			// Set code setup according combination
			foreach (var parameter in combination)
			{
				JToken jToken = jObject.SelectToken(parameter.Key);
				if (parameter.Value is double doubleVariable)
				{
					jToken.Replace(doubleVariable);
				}
				else if (parameter.Value is bool boolVariable)
				{
					jToken.Replace(boolVariable);
				}
				else if (parameter.Value is long longVariable)
				{
					jToken.Replace(longVariable);
				}
				else
				{
					Debug.Assert(false, "unknown type");
				}
			}

			// Update code setup
			client.UpdateCodeSetupJSON(jObject.ToString());
		}

		private List<string> GetAllLoadsFromProject(ConProjectInfo projectInfo)
		{
			List<string> projectLoads = new List<string>();
			foreach (string identifier in projectInfo.Connections.Select(con => con.Identifier).ToList())
			{
				projectLoads.Add(client.GetConnectionLoadingJSON(identifier));
			}
			return projectLoads;
		}

		private void ResetLoads(ConProjectInfo projectInfo, List<string> loads)
		{
			for (int i = 0; i < projectInfo.Connections.Count; i++)
			{
				UpdateLoads(projectInfo.Connections[i].Identifier, loads[i], 1);
			}
		}

		private void UpdateLoads(string identifier, string loads, double loadCoefficient)
		{
			List<LoadEffects> loadEffects = new List<LoadEffects>();
			JsonConvert.PopulateObject(loads, loadEffects);

			// For now only first load effect is considered
			if (loadEffects.Count > 1)
			{
				loadEffects.RemoveRange(1, loadEffects.Count - 1);
			}

			// Update loads
			foreach (var load in loadEffects)
			{
				foreach (var force in load.forcesOnSegments)
				{
					force.n *= loadCoefficient;
					force.mx *= loadCoefficient;
					force.my *= loadCoefficient;
					force.mz *= loadCoefficient;
					force.qy *= loadCoefficient;
					force.qz *= loadCoefficient;
				}
			}

			string updatedLoads = JsonConvert.SerializeObject(loadEffects, Formatting.Indented);
			client.UpdateLoadingFromJson(identifier, updatedLoads);
		}

		private ConnectionResultInfo CalculateUptoMaximumUtilization(ConnectionHiddenCheckClient client, ConnectionInfo con, List<string> stopAtLimitResultItems)
		{
			bool getResults = false;
			double loadCoefficient = 1;
			var count = 0;
			ConnectionResultInfo result = new ConnectionResultInfo();

			// Get initial loads
			var loads = client.GetConnectionLoadingJSON(con.Identifier);

			// Get result to stop at limit
			List<string> resultItemsLimits = stopAtLimitResultItems;

			if (stopAtLimitResultItems.Contains("All"))
			{
				resultItemsLimits = resultsSummaryItems;
			}

			// Generate GoalSeeker for selected result items
			List<GoalSeeker> goalSeekerItems = new List<GoalSeeker>();
			foreach (string item in resultItemsLimits)
			{
				if (item == "Plates")
				{
					goalSeekerItems.Add(new GoalSeeker(5, 0.1));
				}
				else
				{
					goalSeekerItems.Add(new GoalSeeker(100, 0.5));
				}
			}

			// Calculate and stop at limit utilization
			while (!getResults && count <= 100)
			{
				count++;

				// Update loads according load coefficient
				UpdateLoads(con.Identifier, loads, loadCoefficient);

				// Calculate a get results
				double calculationTime;
				var watch = Stopwatch.StartNew();
				var cbfemResults = client.Calculate(con.Identifier);

				// Calculation time in seconds
				watch.Stop();
				calculationTime = watch.ElapsedMilliseconds / 1000.0;

				// Get result summary
				var resultSummary = cbfemResults.ConnectionCheckRes.LastOrDefault().CheckResSummary;

				List<double> loadCoefficients = new List<double>();

				// If any result limits, check its utilization
				if (resultItemsLimits.Any())
				{
					for (int i = 0; i < resultItemsLimits.Count; i++)
					{
						string item = resultItemsLimits[i];
						var goalSeeker = goalSeekerItems[i];

						if (resultSummary.Find(x => x.Name == item) != null)
						{
							var itemSummary = resultSummary.Find(x => x.Name == item).CheckValue;

							// If result utilization within tolerance get result, else find new load coefficient
							if (goalSeeker.IsOutputWithinTolerance(itemSummary))
							{
								getResults = true;
								break;
							}
							else
							{
								goalSeeker.AddData(loadCoefficient, itemSummary);
								loadCoefficients.Add(goalSeeker.SuggestInput());
							}
						}
					}
				}
				// If no limits presented get results
				else
				{
					getResults = true;
				}

				if (!getResults)
				{
					loadCoefficient = loadCoefficients.Min();
				}
				else
				{
					result.Summary = resultSummary;
					result.CalculationTime = calculationTime;
					result.LoadCoefficient = loadCoefficient;
					result.NumberOfIteration = count;
				}

			}

			return result;
		}
	}
}
