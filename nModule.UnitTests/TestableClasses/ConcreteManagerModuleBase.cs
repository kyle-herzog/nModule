using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule;

namespace nModule.UnitTests.TestableClasses
{
    class ConcreteManagerModuleBase<IModule> : ManagerModuleBase<IModule>
    {
        public ConcreteManagerModuleBase() : base() { }
    }

}
