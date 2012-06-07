using System;
using System.Diagnostics;
using Rhino.Mocks;
using Xunit;
using nModule.Utilities;
using nModule.UnitTests.Base;

namespace nModule.UnitTests.Utilities
{
    public class ProcessUtilitySpecs
    {
        const string TestProcess = "cmd";

        public class when_creating_a_simple_process_using_process_utilities : Specification
        {
            Process _process;
            const int RandomLength = 10;

            protected override void Establish_That()
            {
            }

            protected override void Because_Of()
            {
                _process = ProcessUtility.LaunchExternalProcess(Random.NextString(RandomLength), null);
            }

            [Fact]
            public void should_return_a_process_unstarted()
            {
                Assert.Throws<InvalidOperationException>(() => { var startTime = _process.StartTime; } );
            }
        }

        public class when_auto_starting_a_process_using_process_utilities : Specification
        {
            Process _process;
            private DateTime _beforeLaunchExternalProcess;
            private DateTime _afterLaunchExternalProcess;

            protected override void Establish_That() { }

            protected override void Because_Of()
            {
                _beforeLaunchExternalProcess = DateTime.Now;
                _process = ProcessUtility.LaunchExternalProcess(TestProcess, null, true, false, null);
                _afterLaunchExternalProcess = DateTime.Now;
            }

            [Fact]
            public void should_return_a_process_unstarted()
            {
                Assert.True(_beforeLaunchExternalProcess <= _process.StartTime);
                Assert.True(_afterLaunchExternalProcess >= _process.StartTime);
                Assert.DoesNotThrow(_process.Kill);
            }
        }

        public class when_passing_a_process_data_capturer : Specification
        {
            IProcessDataCapturer _processDataCapturer;
            Process _process;
            Process assignedProcess { get; set; }

            protected override void Establish_That()
            {
                _processDataCapturer = Mocker.DynamicMock<IProcessDataCapturer>();
                _processDataCapturer.Expect(x => x.Process).PropertyBehavior();
            }

            protected override void Because_Of()
            {
                _process = ProcessUtility.LaunchExternalProcess(TestProcess, "", true, false, _processDataCapturer);
            }

            [Fact]
            public void should_set_iProcess_data_capturer()
            {
                Assert.NotNull(_processDataCapturer.Process);
            }

            [Fact]
            public void should_set_iProcess_data_capturer_to_process_created()
            {
                Assert.Equal(_process, _processDataCapturer.Process);
                Assert.Equal(_process.Id, _processDataCapturer.Process.Id);
            }
        }

        public class when_creating_a_process_and_waiting_for_the_exit : Specification
        {
            Process _process;

            protected override void Establish_That() { }

            protected override void Because_Of()
            {
                _process = ProcessUtility.LaunchExternalProcess(TestProcess, "/C echo 'Hello World!'", true, true, null);
            }

            [Fact]
            public void should_wait_for_the_program_to_exit()
            {
                Assert.True(_process.HasExited);
            }
        }
    }
}