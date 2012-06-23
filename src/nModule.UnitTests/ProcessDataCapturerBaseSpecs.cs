using System;
using nModule.UnitTests.Base;
using Rhino.Mocks;
using Xunit;
using nModule.Utilities;
using System.Diagnostics;
using System.Text;
using Rhino.Mocks.Interfaces;

namespace nModule.UnitTests
{
    public class ProcessDataCapturerBaseSpecs
    {
        public class TestProcessDataCapturer : ProcessDataCapturerBase
        {
            StringBuilder _outputBuilder;

            public string ProcessOutput { get { return _outputBuilder.ToString(); } }

            public TestProcessDataCapturer()
            {
                _outputBuilder = new StringBuilder();
            }

            protected internal override void Write(string value)
            {
                _outputBuilder.AppendLine(value);
            }
        }

        public class when_capturing_data_using_the_default_base : Specification<TestProcessDataCapturer>
        {
            private Process _process;
            private bool _lastOutputGetSet;

            protected override void Establish_That()
            {
                TestedClass
                    .Stub(tc => tc.Write(Arg<string>.Is.Anything))
                    .CallOriginalMethod(OriginalCallOptions.NoExpectation);
                _process = ProcessUtility.LaunchExternalProcess("ping.exe", "www.google.com");
                _process.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
            }

            protected override void Because_Of()
            {
                TestedClass.Process = _process;
                _process.Start();
                _process.BeginErrorReadLine();
                _process.BeginOutputReadLine();
                _process.WaitForExit();
                _process.Close();
            }

            [Fact]
            public void should_capture_data_from_the_process()
            {
                Console.WriteLine(TestedClass.ProcessOutput);
                Assert.NotNull(TestedClass.ProcessOutput);
                Assert.NotEqual("", TestedClass.ProcessOutput);
            }

            [Fact]
            public void should_set_last_output()
            {
                Assert.True(_lastOutputGetSet);
            }

            #region helpers

            protected void OutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                if (!_lastOutputGetSet)
                    _lastOutputGetSet = TestedClass.LastOutput != null;
            }

            #endregion

        }

    }
}
