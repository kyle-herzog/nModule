using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
	/// <summary>
	/// An enumeration defining how a module may be instantiated.
	/// </summary>
	[Flags]
	public enum ModuleInstantiation
	{
		/// <summary>
		/// Will be automatically instantiated by the ModuleManager upon its initialization
		/// </summary>
		Singleton = 1,
		/// <summary>
		/// Will allow only configured stored Modules to be created from this Module.
		/// </summary>
		Instantiable = 2
	}
}
