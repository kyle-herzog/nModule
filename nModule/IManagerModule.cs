using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
    /// <summary>
    /// An interface providing all the functionality for Managing Module isntances
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public interface IManagerModule<M> : IModule where M : IModule
    {
        /// <summary>
        /// Provides the best Module current instantiated.
        /// </summary>
        M BestModule { get; }
    }
}
