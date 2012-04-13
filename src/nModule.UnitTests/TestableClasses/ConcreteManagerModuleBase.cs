using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule;

namespace nModule.UnitTests.TestableClasses
{
    class ConcreteManagerModuleBase<M> : ManagerModuleBase<M> where M : IModule
    {
        public ConcreteManagerModuleBase() : base() { }
    }

}
