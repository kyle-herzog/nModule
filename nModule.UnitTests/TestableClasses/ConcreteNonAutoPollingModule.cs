using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule.UnitTests.TestableClasses
{
    class ConcreteNonAutoPollingModule : ModuleBase
    {
        public ConcreteNonAutoPollingModule() : base() { }
        public ConcreteNonAutoPollingModule(string name) : base(name) { }

        public override string ModuleType { get { return "ConcreteNonAutoPollingModule"; } }
        public override bool IsAutoPollingModule { get { return false; } }
    }
}
