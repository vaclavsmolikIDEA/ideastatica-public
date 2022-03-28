using IdeaStatiCa.Plugin.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace IdeaStatiCa.Plugin
{
	/// <summary>
	/// Creates the client of connection hidden calculator which communicates using gRPC and runs on the local PC
	/// </summary>
	public class ConnHiddenClientGrpcFactory : IConnCalculatorFactory
	{
		private IPluginLogger PluginLogger { get; set; }

		private readonly string IdeaInstallDir;
		private Process CalculatorProcess { get; set; }
		private Uri CalculatorUrl { get; set; }

#if DEBUG
		private int StartTimeout = -1;
#else
		int StartTimeout = 1000*20;
#endif

		public ConnHiddenClientGrpcFactory(string ideaInstallDir, IPluginLogger pluginLogger)
		{
			Debug.Assert(ideaInstallDir != null);
			Debug.Assert(pluginLogger != null);

			PluginLogger = pluginLogger;
			IdeaInstallDir = ideaInstallDir;
		}

		/// <inheritdoc cref="IConnCalculatorFactory.Create"/>
		public IConnHiddenCheck Create()
		{

			return null;
		}


		private void RunCalculatorProcess()
		{
			if (CalculatorProcess != null)
			{
				return;
			}

			int myProcessId = Process.GetCurrentProcess().Id;

			string eventName = string.Format(Constants.ConCalculatorChangedEventFormat, myProcessId);
			using (EventWaitHandle syncEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName))
			{
				string connChangedEventName = string.Format(Constants.ConCalculatorChangedEventFormat, myProcessId);
				string applicationExePath = Path.Combine(IdeaInstallDir, "IdeaStatiCa.ConnHiddenCalculator.exe");

				if (!File.Exists(applicationExePath))
				{
					throw new ArgumentException($"RunCalculatorProcess - file '{applicationExePath}' doesn't exists");
				}

				int grpcPort = PortFinder.FindPort();

				string cmdParams = $"-automation{myProcessId} {Constants.GrpcPortParam}:{grpcPort}";
				ProcessStartInfo psi = new ProcessStartInfo(applicationExePath, cmdParams);
				psi.WindowStyle = ProcessWindowStyle.Normal;
				psi.UseShellExecute = false;

#if DEBUG
				psi.CreateNoWindow = false;
#else
				psi.CreateNoWindow = true;
#endif

				CalculatorProcess = new Process();
				CalculatorProcess.StartInfo = psi;
				CalculatorProcess.EnableRaisingEvents = true;
				CalculatorProcess.Start();

				if (!syncEvent.WaitOne(StartTimeout))
				{
					throw new TimeoutException($"Timeout - the process '{applicationExePath}' doesn't set the event '{eventName}'");
				}
			}

			//ConnectionHiddenCheckClient.HiddenCalculatorId = CalculatorProcess.Id;
			//CalculatorUrl = new Uri(string.Format(Constants.ConnHiddenCalculatorUrlFormat, CalculatorProcess.Id));
			CalculatorProcess.Exited += CalculatorProcess_Exited;
		}

		private void CalculatorProcess_Exited(object sender, EventArgs e)
		{
			if (CalculatorProcess == null)
			{
				return;
			}

			ConnectionHiddenCheckClient.HiddenCalculatorId = -1;
			CalculatorProcess.Dispose();
			CalculatorProcess = null;
		}

	}
}
