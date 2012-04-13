using System;

namespace nModule
{
	/// <summary>
	/// 
	/// </summary>
    public interface IModule : IDisposable
    {
		/// <summary>
		/// Provides a name that is displayed for the Module
		/// </summary>
        string ModuleName { get; }
		/// <summary>
		/// An Id that uniquely identifies the module.
		/// </summary>
		int ModuleId { get; }
		/// <summary>
		/// How a module may be instantiated by the ModuleManager
		/// </summary>
		ModuleInstantiation ModuleInstantiation { get; }
		/// <summary>
		/// A string value depicting the type of Module
		/// </summary>
		string ModuleType { get; }
		/// <summary>
		/// The priority of the module used to 
		/// </summary>
		int ModulePriority { get; set; }
		/// <summary>
		/// A description of the module's current status.
		/// </summary>
		string ModuleStatus { get; }
		/// <summary>
		/// The current state of the Module.
		/// </summary>
		ModuleState ModuleState { get; }
		/// <summary>
		/// Initializes the Module to a working state.
		/// </summary>
		void Initialize();
		/// <summary>
		/// Polls the module to force an update of its ModuleStatus
		/// </summary>
		void Poll();
		/// <summary>
		/// Value stating whether or not the Module is being Polled.
		/// </summary>
		bool IsPolling { get; }
		/// <summary>
		/// Value stating whether or not the Module should initiave a PollingThread
		/// </summary>
		bool IsAutoPollingModule { get; }
		/// <summary>
		/// How often, in milliseconds, the Module with automatically poll itself.
		/// </summary>
		int ModuleAutoPollFrequency { get; set; }

		//TODO IsPolling
		//TODO IsAutoPollingModule
		//TODO PollWaitTime
    }
}
