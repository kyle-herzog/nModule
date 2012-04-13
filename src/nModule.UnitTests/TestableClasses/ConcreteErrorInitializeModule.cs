using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule.UnitTests.TestableClasses
{
    class ConcreteErrorInitializeModule : ModuleBase
    {
        public ConcreteErrorInitializeModule() : base() { }
        public ConcreteErrorInitializeModule(string name) : base(name) { }

        public override string ModuleType
        {
            get { return "Concrete Module"; }
        }

        protected internal override void OnInitialize()
        {
            throw new NotImplementedException();
        }
    }

}
