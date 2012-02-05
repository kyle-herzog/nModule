using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
    /// <summary>
    /// An interface providing all the functionality for Managing Module isntances
    /// </summary>
    /// <typeparam name="IModule"></typeparam>
    public interface IManagerModule<IModule> : IModule
    {
        /// <summary>
        /// Provides the best Module current instantiated.
        /// </summary>
        IModule BestModule { get; }
    }
}
