using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionParametrizationExample.Services
{
	public class GoalSeeker
	{
		// Load coefficient
		List<double> inputData = new List<double>();

		// Model utilization
		List<double> outputData = new List<double>();

		bool isIncreasing = true;
		double targetOutput;
		double tolerance;
		double startInput;
		double basicInputIncrement;

		//GoalSeeker(double tolUt = 0.05, double tolStrain = 0.01)
		public GoalSeeker(double targetOutput, double tolerance, double startInput=1, double basicInputIncrement=0.5)
		{
			this.targetOutput = targetOutput;
			this.tolerance = tolerance;
			this.startInput = startInput;
			this.basicInputIncrement = basicInputIncrement;
		}

		public void AddData(double input, double output)
		{
			inputData.Add(input);
			inputData = inputData.OrderBy(x => x).ToList();
			int index = inputData.IndexOf(input);
			outputData.Insert(index, output);

			// Check if increasing
			for (int i = 1; i < outputData.Count; i++)
			{
				if (outputData[i - 1] > outputData[i])
				{
					isIncreasing = false;
					return;
				}
			}
		}

		public bool IsOutputWithinTolerance(double output)
		{
			return Math.Abs(output - targetOutput) <= tolerance;
		}

		public double SuggestInput()
		{
			double suggestedInput;

			if (inputData.Count == 0)
			{
				suggestedInput = startInput;
			}
			else if(inputData.Count == 1)
			{
				double coefficient = targetOutput / outputData[0];
				suggestedInput = coefficient * inputData[0];
			}
			// All output is to high
			else if(targetOutput < outputData[0])
			{
				// Output is not changing, cannot
				if (outputData[0] == outputData[1])
				{
					if (isIncreasing)
					{
						suggestedInput = outputData[0] - (basicInputIncrement * outputData.Count);
					}
					else
					{
						suggestedInput = outputData[0] + (basicInputIncrement * outputData.Count);
					}
				}
				else
				{
					double in1 = inputData[0];
					double in2 = inputData[1];
					double out1 = outputData[0];
					double out2 = outputData[1];
					suggestedInput = in1 + (in2 - in1) * (targetOutput - out1) / (out2 - out1);
				}
			}
			// All output is to low
			else if (targetOutput > outputData.Last())
			{
				// Output is not changing, cannot
				if (outputData.Last() == outputData[outputData.Count - 2])
				{
					if (isIncreasing)
					{
						suggestedInput = outputData.Last() + (basicInputIncrement * outputData.Count);
					}
					else
					{
						suggestedInput = outputData.Last() - (basicInputIncrement * outputData.Count);
					}
				}
				else
				{
					double in1 = inputData.Last();
					double in2 = inputData[inputData.Count - 2];
					double out1 = outputData.Last();
					double out2 = outputData[inputData.Count - 2];
					suggestedInput = in1 + (in2 - in1) * (targetOutput - out1) / (out2 - out1);
				}
			}
			// Determine suggested input using Newton's method between two nearest outputs.
			else
			{
				int index = outputData.FindIndex(item => item >= targetOutput);
				int index1;
				int index2;
				double distance1 = Math.Abs(outputData[index] - targetOutput);
				double distance2 = Math.Abs(outputData[index - 1] - targetOutput);

				if (distance1 < distance2)
				{
					index1 = index;
					if(index == inputData.Count - 1)
					{
						index2 = index - 1;
					}
					else
					{
						double distance3 = Math.Abs(outputData[index + 1] - targetOutput);
						if(distance3 < distance2)
						{
							index2 = index + 1;
						}
						else
						{
							index2 = index - 1;
						}
					}
				}
				else
				{
					index1 = index - 1;
					if (index == 1)
					{
						index2 = index;
					}
					else
					{
						double distance3 = Math.Abs(outputData[index - 2] - targetOutput);
						if(distance3 < distance1)
						{
							index2 = index - 2;
						}
						else
						{
							index2 = index;
						}
					}
				}
				double in1 = inputData[index1];
				double in2 = inputData[index2];
				double out1 = outputData[index1];
				double out2 = outputData[index2];
				suggestedInput = in1 + (in2 - in1) * (targetOutput - out1) / (out2 - out1);

				if(suggestedInput >= inputData[index])
				{
					suggestedInput = 0.5 * (inputData[index - 1] + inputData[index]);
				}
				if(suggestedInput <= inputData[index - 1])
				{
					suggestedInput = 0.5 * (inputData[index - 1] + inputData[index]);
				}
			}
			return suggestedInput;
		}
	}
}
