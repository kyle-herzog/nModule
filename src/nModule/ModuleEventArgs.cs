using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
	/// <summary>
	/// Common arguments for events triggered from Modules
	/// </summary>
	public class ModuleEventArgs : EventArgs
	{
		/// <summary>
		/// The Name of the Module which triggered the event.
		/// </summary>
		public string ModuleName { get; private set; }
		/// <summary>
		/// The Id of the Module which triggered the event.
		/// </summary>
		public int ModuleId { get; private set; }
		internal int ModuleThreadId { get; private set; }
		internal string ModuleThreadName { get; private set; }

		/// <summary>
		/// Initialized the ModuleEventArgs with the given moduleName and moduleId
		/// </summary>
		/// <param name="moduleName"></param>
		/// <param name="moduleId"></param>
		public ModuleEventArgs(string moduleName, int moduleId)
		{
			ModuleName = moduleName;
			ModuleId = moduleId;
		}

		internal ModuleEventArgs(string moduleName, int moduleId, string moduleThreadName, int moduleThreadId)
			: this(moduleName, moduleId)
		{
			ModuleThreadId = moduleThreadId;
			ModuleThreadName = moduleThreadName;
		}
	}
}
