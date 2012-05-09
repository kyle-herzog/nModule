using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nModule.UnitTests.Base{
    public abstract class TestBase<T> : TestBase where T : class
    {
        protected T TestedClass { get; private set; }

        public TestBase() : base()
        {
            TestedClass = Mocker.PartialMock<T>();
        }
    }
}
