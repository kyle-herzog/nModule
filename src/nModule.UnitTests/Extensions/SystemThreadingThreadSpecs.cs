using System.Threading;
using Xunit;
using nModule.UnitTests.Base;

namespace nModule.UnitTests.Extensions
{
    public class SystemThreadingThreadSpecs
    {
        public abstract class when_using_a_thread : Specification
        {
            protected Thread Thread;

            protected override void Establish_That()
            {
                Thread = new Thread(() =>
                {
                    while (true)
                        Thread.Sleep(0);
                });
            }

            protected override void Because_Of()
            {
                Thread.Start();
            }

        }

        public class when_aborting_a_thread_using_custom_extension_method : when_using_a_thread
        {
            [Fact]
            public void should_abort_the_thread_catching_the_thread_abort_exception()
            {
                Assert.DoesNotThrow(Thread.AbortSafely);
            }
        }

        public class when_checking_for_a_running_thread : when_using_a_thread
        {
            [Fact]
            public void should_return_running()
            {
                Assert.Equal<bool>(true, Thread.IsThreadRunning());
                Thread.AbortSafely();
            }

            [Fact]
            public void should_return_not_running()
            {
                Thread.AbortSafely();
                Assert.Equal<bool>(false, Thread.IsThreadRunning());
            }
        }
    }
}