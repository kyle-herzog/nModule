using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule.UnitTests.TestableClasses
{
    class ConcreteSlowDisposingModule : ModuleBase
    {
        public ConcreteSlowDisposingModule() : base() { }
        public ConcreteSlowDisposingModule(string name) : base(name) { }

        public override string ModuleType
        {
            get { return "Concrete Module"; }
        }
    }
}
