using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule.UnitTests.TestableClasses
{
    class ConcreteModule : ModuleBase
    {
        public ConcreteModule() : base() { }
        public ConcreteModule(string name) : base(name) { }

        public override string ModuleType
        {
            get { return "Concrete Module"; }
        }
    }
}
