﻿using System.Threading.Tasks;

namespace IdeaStatiCa.Plugin
{
	/// <summary>
	/// Responsible for controlling the connected BIM application to IS
	/// </summary>
	/// <typeparam name="T">Type of the plugin's service contract</typeparam>
	public interface IBIMPluginClient<T>
	{
		/// <summary>
		/// Used for calling methods of connected BIM application
		/// </summary>
		T MyBIM { get; }

		/// <summary>
		/// Notification about events in the connected BIM application
		/// </summary>
		event ISEventHandler BIMStatusChanged;

		/// <summary>
		/// Starts BIM application
		/// </summary>
		/// <param name="id">Identified of </param>
		/// <returns></returns>
		Task RunAsync(string id);

		/// <summary>
		/// Stops BIM application
		/// </summary>
		void Stop();

		/// <summary>
		/// Get status of the BIM application
		/// </summary>
		AutomationStatus Status
		{
			get;
		}
	}
}
