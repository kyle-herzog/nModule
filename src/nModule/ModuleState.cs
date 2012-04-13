using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
	/// <summary>
	/// The possible states of a Module.
	/// </summary>
	public enum ModuleState
	{
		/// <summary>
		/// Module is not initialized
		/// </summary>
		NotInitialized,
		/// <summary>
		/// Module has been Disposed
		/// </summary>
		Disposed,
		/// <summary>
		/// Module is Initializing
		/// </summary>
		Initializing,
		/// <summary>
		/// Module is working as expected
		/// </summary>
		Healthy,
		/// <summary>
		/// Module has an error that is possible to bypass via configuration
		/// </summary>
		Warning,
		/// <summary>
		/// Module has received an unexpected error
		/// </summary>
		Error,
		/// <summary>
		/// A state where the Module is Initialized but depends on another module that is not Initialized yet
		/// </summary>
		InitializationDepending
	}
}
