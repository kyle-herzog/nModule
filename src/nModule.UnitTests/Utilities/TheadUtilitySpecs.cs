using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule.UnitTests.Base;
using nModule.Utilities;
using System.Threading;
using Xunit;

namespace nModule.UnitTests.Utilities
{
    public class TheadUtilitySpecs
    {
        public abstract class and_using_Thread_Utils : Specification
        {
            protected const int MinWaitTime = 10;
            protected bool KeepThreadRunning = true;
            protected Thread Thread;
            protected bool TestThreadStartCalled;
            protected bool TestParameterizedThreadStartCalled;

            protected void TestThreadStart()
            {
                TestThreadStartCalled = true;
                while (KeepThreadRunning)
                {
                    Thread.Sleep(10);
                }
            }
            
            protected void TestParameterizedThreadStart(object obj)
            {
                TestParameterizedThreadStartCalled = true;
                while (KeepThreadRunning)
                {
                    Thread.Sleep(10);
                }
            }
        }

        public class when_creating_thread_using_thread_utils : and_using_Thread_Utils
        {
            string _threadName;

            protected override void Establish_That()
            {
                _threadName = Random.NextString(RandomStringSize);
            }

            protected override void Because_Of()
            {
                Thread = ThreadUtility.CreateThread(TestThreadStart, _threadName);
            }

            [Fact]
            public void should_name_the_thread()
            {
                Assert.Equal(_threadName, Thread.Name);
                KeepThreadRunning = false;
            }

            [Fact]
            public void should_start_the_thread()
            {   
                Assert.True(Thread.IsAlive);
                KeepThreadRunning = false;
            }

            [Fact]
            public void should_make_it_a_background_thread()
            {
                Assert.True(Thread.IsBackground);
                KeepThreadRunning = false;
            }

            [Fact]
            public void should_set_priority_to_normal()
            {
                Assert.Equal(ThreadPriority.Normal, Thread.Priority);
                KeepThreadRunning = false;
            }
        }

        public class when_creating_a_parameterized_thread_using_thread_utils : and_using_Thread_Utils
        {
            string _threadName;

            protected override void Establish_That()
            {
                _threadName = Random.NextString(RandomStringSize);
            }

            protected override void Because_Of()
            {
                Thread = ThreadUtility.CreateThread(TestParameterizedThreadStart, _threadName, false);
            }

            [Fact]
            public void should_name_the_thread()
            {
                Assert.Equal(_threadName, Thread.Name);
                KeepThreadRunning = false;
            }

            [Fact]
            public void should_start_the_thread()
            {
                Assert.True(Thread.IsAlive);
                KeepThreadRunning = false;
            }

            [Fact]
            public void should_make_it_a_background_thread()
            {
                Assert.True(Thread.IsBackground);
                KeepThreadRunning = false;
            }

            [Fact]
            public void should_set_priority_to_normal()
            {
                Assert.Equal(ThreadPriority.Normal, Thread.Priority);
                KeepThreadRunning = false;
            }
        }
    }
}
