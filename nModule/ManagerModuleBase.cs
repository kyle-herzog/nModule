using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule
{
    /// <summary>
    /// Generic Module Manager base implementation
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    public abstract class ManagerModuleBase<T> : IManagerModule<T>
    {
        public string ModuleName
        {
            get { throw new NotImplementedException(); }
        }

        public int ModuleId
        {
            get { throw new NotImplementedException(); }
        }

        public ModuleInstantiation ModuleInstantiation
        {
            get { throw new NotImplementedException(); }
        }

        public string ModuleType
        {
            get { throw new NotImplementedException(); }
        }

        public int ModulePriority
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ModuleStatus
        {
            get { throw new NotImplementedException(); }
        }

        public ModuleState ModuleState
        {
            get { throw new NotImplementedException(); }
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Poll()
        {
            throw new NotImplementedException();
        }

        public bool IsPolling
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAutoPollingModule
        {
            get { throw new NotImplementedException(); }
        }

        public int ModuleAutoPollFrequency
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
