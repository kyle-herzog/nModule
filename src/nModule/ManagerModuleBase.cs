using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
    /// <summary>
    /// Generic Module Manager base implementation
    /// </summary>
    /// <typeparam name="M">The type of Module to be managed</typeparam>
    public abstract class ManagerModuleBase<M> : ModuleBase, IManagerModule<M> where M : IModule
    {
        private string _typeName;
        private string _moduleType;
        /// <summary>
        /// Gets the best module.
        /// </summary>
        public M BestModule
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// When overridden will provide a value describing the module
        /// </summary>
        public override string ModuleType
        {
            get { return _moduleType; }
        }

        /// <summary>
        /// Provides a base initialization of new instances of the <see cref="ManagerModuleBase&lt;IModule&gt;"/> class.
        /// </summary>
        public ManagerModuleBase()
        {
            _typeName = typeof(IModule).Name;
            _moduleType = String.Format("{0} Manager", _typeName);
        }

    }
}
