using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
    /// <summary>
    /// Constants values for various states of a Module
    /// </summary>
    public class ModuleStatusConstants
    {
        /// <summary>
        /// String for stating the Module is newly instantiated
        /// </summary>
        public const string Instantiated = "Instantiated";

        /// <summary>
        /// String for stating the Module is Initializing
        /// </summary>
        public const string Initializing = "Initializing";

        /// <summary>
        /// String for stating the Module has failed to initialize.
        /// </summary>
        public const string InitializeError = "An error occurred when initializing the module";

        /// <summary>
        /// String for stating the Module is Initialized
        /// </summary>
        public const string Initialized = "The module is Initialized and ready for use";

        /// <summary>
        /// String stating the Module errored upon disposing
        /// </summary>
        public const string DisposeError = "An error occured when disposing the module";

        /// <summary>
        /// String stating the Module is in an errored state
        /// </summary>
        public const string Error = "An error occured when polling the module.";
    }
}
