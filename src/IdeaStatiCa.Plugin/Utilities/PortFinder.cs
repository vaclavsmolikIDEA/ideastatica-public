using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace IdeaStatiCa.Plugin.Utilities
{
	/// <summary>
	/// Tool used to find a free port for gRPC communication.
	/// </summary>
	public static class PortFinder
	{
		/// <summary>
		/// Searches for the next available port.
		/// </summary>
		/// <param name="minPort">Initial port to start the search from.</param>
		/// <param name="maxPort"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="Exception"></exception>
		public static int FindPort(int minPort = Constants.MinGrpcPort, int maxPort = Constants.MaxGrpcPort)
		{
			if (maxPort < minPort)
			{
				throw new ArgumentException("Max cannot be less than min.");
			}

			var ipProperties = IPGlobalProperties.GetIPGlobalProperties();

			var usedPorts =
				ipProperties.GetActiveTcpConnections()
					.Where(connection => connection.State != TcpState.Closed)
					.Select(connection => connection.LocalEndPoint)
					.Concat(ipProperties.GetActiveTcpListeners())
					.Concat(ipProperties.GetActiveUdpListeners())
					.Select(endpoint => endpoint.Port)
					.ToArray();

			var firstUnused =
				Enumerable.Range(minPort, maxPort - minPort)
					.Where(port => !usedPorts.Contains(port))
					.Select(port => new int?(port))
					.FirstOrDefault();

			if (!firstUnused.HasValue)
				throw new Exception($"All local TCP ports between {minPort} and {maxPort} are currently in use.");

			return firstUnused.Value;
		}
	}
}
