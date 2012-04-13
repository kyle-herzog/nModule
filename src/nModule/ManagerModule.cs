using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
    /// <summary>
    /// A ManagerModule for all Modules.
    /// </summary>
    /// <typeparam name="M">The type of the module.</typeparam>
    public class ManagerModule<M> : ManagerModuleBase<M> where M : IModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerModule&lt;IModule&gt;"/> class.
        /// </summary>
        public ManagerModule() : base()
        {

        }
    }
}
