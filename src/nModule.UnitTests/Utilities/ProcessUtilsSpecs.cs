using System;
using System.Diagnostics;
using Xunit;
using nModule.Utilities;
using nModule.UnitTests.Base;

namespace nModule.UnitTests.Utilities
{
    public class ProcessUtilsSpecs
    {
        public class when_creating_a_simple_process_using_process_utilities : Specification
        {
            Process _process;
            const int RandomLength = 10;

            protected override void Establish_That()
            {
            }

            protected override void Because_Of()
            {
                _process = ProcessUtils.LaunchExternalProcess(Random.NextString(RandomLength), null);
            }

            [Fact]
            public void should_return_a_process_unstarted()
            {
                Assert.Throws<InvalidOperationException>(() => { var startTime = _process.StartTime; } );
            }
        }
    }
}