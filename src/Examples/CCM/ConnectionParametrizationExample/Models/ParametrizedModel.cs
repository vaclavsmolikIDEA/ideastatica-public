﻿using IdeaStatiCa.Plugin;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using ConnectionParametrizationExample.Extensions;
using System.ComponentModel;
using ConnectionParametrizationExample.Services;
using IdeaRS.OpenModel.Connection;

namespace ConnectionParametrizationExample.Models
{
	public class ParametrizedModel
	{
		public string IdeaAppLocation { get; set; }
		public string IdeaConFilesLocation { get; set; }
		private ConnectionHiddenCheckClient client;

		public void RunParametrizedAnalysis(List<string> IdeaConFiles, BackgroundWorker worker)
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
								// Iterate all connections in the project
								foreach (var con in projInfo.Connections)
								{
									UpdateCodeSetup(combination.Value);

									double calculationTime = 0;
									//CalculateUptoMaximumUtilization(client, con, calculationTime);
									var watch = Stopwatch.StartNew();

									// Calculate a get results
									var cbfemResults = client.Calculate(con.Identifier);

									// Calculation time in seconds
									watch.Stop();
									calculationTime = watch.ElapsedMilliseconds / 1000.0;

									// Get result summary
									var resultSummary = cbfemResults.ConnectionCheckRes.LastOrDefault().CheckResSummary;

									// Add result to builder
									string connectionName = $"{Path.GetFileNameWithoutExtension(ideaConFile)}_{con.Name}";
									resultBuilder.AddResult(connectionName, calculationTime, resultSummary, combination.Value.ToList(), combination.Index);
								}
								// Save Project
								string calculatedModels = "CalculatedModels";
								if (!Directory.Exists(Path.Combine(IdeaConFilesLocation, calculatedModels)))
								{
									Directory.CreateDirectory(Path.Combine(IdeaConFilesLocation, calculatedModels));
								}
								string newProjectPath = Path.Combine(IdeaConFilesLocation, calculatedModels, $"{Path.GetFileNameWithoutExtension(ideaConFile)}_{combination.Index}.ideaCon");
								client.SaveAsProject(newProjectPath);
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

			File.WriteAllText(Path.Combine(IdeaConFilesLocation, "a.json"), codeSetup);

			// Create new code setup object
			CodeSetup setup = new CodeSetup();
			JsonConvert.PopulateObject(codeSetup, setup);

			// Set code setup according combination
			foreach (var parameter in combination)
			{
				// Set value to parametrized property
				var parametrizedProperty = setup.GetType().GetProperty(parameter.Key);
				parametrizedProperty.SetValue(setup, parameter.Value, null);
			}
			string parametrizedSetup = JsonConvert.SerializeObject(setup, Formatting.Indented);

			// Update code setup
			client.UpdateCodeSetupJSON(parametrizedSetup);

			File.WriteAllText(Path.Combine(IdeaConFilesLocation, "b.json"), client.GetCodeSetupJSON());
		}

		private void CalculateUptoMaximumUtilization(ConnectionHiddenCheckClient client, ConnectionInfo con, double calculationTime)
		{
			bool resultWithinTolerance = false;
			GoalSeeker goalSeeker = new GoalSeeker(1, 0.05);

			while (!resultWithinTolerance)
			{
				var watch = Stopwatch.StartNew();

				// Calculate a get results
				var cbfemResults = client.Calculate(con.Identifier);

				// Calculation time in seconds
				watch.Stop();
				calculationTime = watch.ElapsedMilliseconds / 1000.0;

				// Get result summary
				var resultSummary = cbfemResults.ConnectionCheckRes.LastOrDefault().CheckResSummary;

				var weldSummary = resultSummary.Find(x => x.Name == "Welds").CheckValue;

				Debug.Assert(true);
			}
		}
	}
}
