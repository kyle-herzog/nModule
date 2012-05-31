using Rhino.Mocks;
using System;

namespace nModule.UnitTests.Base
{
    public abstract class TestBase
    {
        protected MockRepository Mocker { get; private set; }
        protected Random Random { get; private set; }
        protected const int RandomStringSize = 10;

        public TestBase()
        {
            Mocker = new MockRepository();
            Random = new Random();
        }
    }
}
