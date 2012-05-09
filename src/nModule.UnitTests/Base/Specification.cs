using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule.UnitTests.Base
{
    public abstract class Specification : TestBase
    {
        protected Specification()
        {
            Setup();
        }

        void Setup()
        {
            Establish_That();
            Because_Of();
        }

        protected abstract void Establish_That(); 
        protected abstract void Because_Of(); 
    }
}
