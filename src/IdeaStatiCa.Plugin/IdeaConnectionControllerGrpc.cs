﻿using IdeaStatiCa.Plugin.Grpc.Reflection;
using IdeaStatiCa.Plugin.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace IdeaStatiCa.Plugin
{
	public class IdeaConnectionControllerGrpc : IDisposable, IConnectionController
	{
		private readonly string IdeaInstallDir;
		private Process IdeaStatiCaProcess { get; set; }
		private Uri CalculatorUrl { get; set; }

		private int GrpcPort { get; set; }

		protected EventWaitHandle CurrentItemChangedEvent;

		protected IdeaStatiCaClient<IAutomation> ConnectionAppClient { get; set; }

		protected AutomationHostingGrpc<IAutomation, IAutomation> GrpcClient { get; set; }

		protected virtual uint UserMode { get; } = 0;

		private string BaseAddress { get; set; }

		bool IConnectionController.IsConnected => GrpcClient?.IsConnected == true;

#if DEBUG
		private int StartTimeout = -1;
#else
		int StartTimeout = 1000*20;
#endif

		private IdeaConnectionControllerGrpc(string ideaInstallDir)
		{
			if(!Directory.Exists(ideaInstallDir))
			{
				throw new ArgumentException($"IdeaConnectionController.IdeaConnectionController - directory '{ideaInstallDir}' doesn't exist");
			}

			IdeaInstallDir = ideaInstallDir;
		}

		public static IConnectionController Create(string ideaInstallDir)
		{
			IdeaConnectionControllerGrpc connectionController = new IdeaConnectionControllerGrpc(ideaInstallDir);
			connectionController.OpenConnectionClient();
			return connectionController;
		}

		public int OpenProject(string fileName)
		{
			GrpcClient.MyBIM.OpenProject(fileName);
			return 1;
		}

		public int CloseProject()
		{
			GrpcClient.MyBIM.CloseProject();
			return 1;
		}

		private void OpenConnectionClient()
		{
			OpenConnectionClientGrpc();
		}

		private void OpenConnectionClientGrpc()
		{
			int processId = Process.GetCurrentProcess().Id;
			GrpcPort = PortFinder.FindPort(50000, 50500);
			string connChangedEventName = string.Format(Constants.ConnectionChangedEventFormat, processId);
			CurrentItemChangedEvent = new EventWaitHandle(false, EventResetMode.AutoReset, connChangedEventName);

			string applicationExePath = Path.Combine(IdeaInstallDir, Constants.IdeaConnectionAppName);

			if (!File.Exists(applicationExePath))
			{
				throw new ArgumentException($"IdeaConnectionController.OpenConnectionClient - file '{applicationExePath}' doesn't exist");
			}

			Process connectionProc = new Process();
			string eventName = string.Format("IdeaStatiCaEvent{0}", processId);
			using (EventWaitHandle syncEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName))
			{
				connectionProc.StartInfo = new ProcessStartInfo(applicationExePath, $"-cmd:automation-{processId} {IdeaStatiCa.Plugin.Constants.GrpcPortParam}:{GrpcPort} user-mode 192");
				connectionProc.EnableRaisingEvents = true;
				connectionProc.Start();

				if (!syncEvent.WaitOne(StartTimeout))
				{
					throw new TimeoutException($"Time out - process '{applicationExePath}' doesn't set the event '{eventName}'");
				}
			}

			IdeaStatiCaProcess = connectionProc;

			GrpcClient = new AutomationHostingGrpc<IAutomation, IAutomation>(null, GrpcPort);
			GrpcClient.RunAsync(processId.ToString());

			IdeaStatiCaProcess.Exited += CalculatorProcess_Exited;
		}

		private async void InitializeGrpc(string clientId, int grpcPort)
		{
			var grpcClient = new GrpcReflectionClient(clientId, grpcPort);

			await grpcClient.ConnectAsync();

			//Actions.Add($"GRPC server connected");
		}

		private void CalculatorProcess_Exited(object sender, EventArgs e)
		{
			if (IdeaStatiCaProcess == null)
			{
				return;
			}

			IdeaStatiCaProcess.Dispose();
			IdeaStatiCaProcess = null;
			CalculatorUrl = null;
			ConnectionAppClient = null;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ConnectionControllerFactory()
		// {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
