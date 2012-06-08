using System;
using System.Threading;
using nModule.Utilities;
using nModule.UnitTests.Base;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace nModule.UnitTests
{
    public class ModuleBaseSpecs
    {
        public class when_creating_a_module_base : Specification<ModuleBase>
        {
            protected override void Establish_That()
            {
                TestedClass.Expect(tc => tc.ModuleInstantiation).CallOriginalMethod(OriginalCallOptions.NoExpectation);
                TestedClass.Expect(x => x.IsAutoPollingModule).CallOriginalMethod(OriginalCallOptions.NoExpectation);
                TestedClass.Expect(x => x.ModuleType).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            }

            protected override void Because_Of() { }

            [Fact]
            public void should_generate_a_module_id()
            {
                Assert.NotEqual<int>(TestedClass.ModuleId, 0);
            }

            [Fact]
            public void should_have_default_module_instantiation_of_singleton()
            {
                Assert.Equal<ModuleInstantiation>(ModuleInstantiation.Singleton, TestedClass.ModuleInstantiation);
            }

            [Fact]
            public void should_default_Module_Type_to_Class_Type()
            {
                Assert.Equal<string>(TestedClass.GetType().Name, TestedClass.ModuleType);
            }

            [Fact]
            public void should_default_module_priority_to_zero()
            {
                Assert.Equal<int>(0, TestedClass.ModulePriority);
            }

            [Fact]
            public void should_set_module_status_to_instantiated()
            {
                Assert.Equal<string>(ModuleStatusConstants.Instantiated, TestedClass.ModuleStatus);
            }

            [Fact]
            public void should_set_module_state_to_not_initialized()
            {
                Assert.Equal<ModuleState>(ModuleState.NotInitialized, TestedClass.ModuleState); 
            }

            [Fact]
            public void should_set_module_auto_poll_frequency_to_one_thousand()
            {
                Assert.Equal<int>(1000, TestedClass.ModuleAutoPollFrequency);
            }

            [Fact]
            public void should_set_is_disposed_to_false()
            {
                Assert.Equal<bool>(false, TestedClass.IsDisposed);
            }

            [Fact]
            public void should_set_is_disposing_to_false()
            {
                Assert.Equal<bool>(false, TestedClass.IsDisposing);
            }

            [Fact]
            public void should_set_is_polling_to_false()
            {
                Assert.Equal<bool>(false, TestedClass.IsPolling);
            }

            [Fact]
            public void should_set_is_auto_polling_module_to_true()
            {
                Assert.Equal<bool>(true, TestedClass.IsAutoPollingModule);
            }

            [Fact]
            public void should_set_module_auto_poll_frequency_to_zero()
            {
                Assert.Equal<int>(1000, TestedClass.ModuleAutoPollFrequency);
            }
        }

        public class when_calling_Internal_initialize : Specification<ModuleBase>
        {
            protected override void Establish_That()
            {
                TestedClass.Expect(tc => tc.InternalInitialize()).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            }

            protected override void Because_Of() { }

            [Fact]
            public void should_not_throw_exceptions()
            {
                Assert.DoesNotThrow(TestedClass.InternalInitialize);
            }
        }

        public class when_calling_Internal_Dispose : Specification<ModuleBase>
        {
            protected override void Establish_That()
            {
                TestedClass.Expect(tc => tc.InternalDispose()).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            }

            protected override void Because_Of() { }

            [Fact]
            public void should_not_throw_exceptions()
            {
                Assert.DoesNotThrow(TestedClass.InternalDispose);
            }
        }

        public class when_creating_a_module_base_with_a_name : Specification
        {
            ModuleBase testedClass;
            string moduleName; 

            protected override void Establish_That()
            {
                moduleName = Random.NextString(10);
                testedClass = Mocker.StrictMock<ModuleBase>(moduleName);
            }

            protected override void Because_Of() { }

            [Fact]
            public void should_assign_the_correct_module_name()
            {
                Assert.Equal<string>(testedClass.ModuleName, moduleName);
            }
        }

        public class when_initializing_a_module_base_class : Specification<ModuleBase>
        {
            protected override void Establish_That() 
            {
                TestedClass.Stub(x => x.IsAutoPollingModule).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            }

            protected override void Because_Of()
            {
                TestedClass.Initialize();
            }

            [Fact]
            public void should_set_the_module_state_to_healthy()
            {
                Assert.Equal<ModuleState>(ModuleState.Healthy, TestedClass.ModuleState);
            }

            [Fact]
            public void should_set_module_status_to_initialized()
            {
                Assert.Equal<string>(ModuleStatusConstants.Initialized, TestedClass.ModuleStatus);
            }

            [Fact]
            public void should_instanciate_Module_Polling_Thread()
            {
                Assert.NotNull(TestedClass.ModulePollingThread);
            }
        }

        public class when_initializing_a_module_base_class_with_on_initialize_overridden : Specification
        {
            const int Wait = 100;
            const int TestBeginWait = Wait/10;
            const int TestEndWait = Wait*2;

            class ConcreateModule : ModuleBase
            {
                protected internal override void InternalInitialize()
                {
                    Thread.Sleep(Wait);
                }
            }

            ModuleBase _testedClass;
            Thread _thread;

            protected override void Establish_That()
            {
                _testedClass = new ConcreateModule();
            }

            protected override void Because_Of()
            {
                _thread = ThreadUtility.CreateThread(_testedClass.Initialize);
            }

            [Fact]
            public void should_set_module_state_to_initializing()
            {
                Thread.Sleep(TestBeginWait);
                Assert.Equal<ModuleState>(ModuleState.Initializing, _testedClass.ModuleState);
            }

            [Fact]
            public void should_set_module_status_to_initializing()
            {
                Thread.Sleep(TestBeginWait);
                Assert.Equal<string>(ModuleStatusConstants.Initializing, _testedClass.ModuleStatus);
            }

            [Fact]
            public void should_eventually_set_state_to_healthy()
            {
                _thread.Join();
                Assert.Equal<ModuleState>(ModuleState.Healthy, _testedClass.ModuleState);
            }

            [Fact]
            public void should_set_the_status_to_healthy()
            {
                _thread.Join();
                Assert.Equal<string>(ModuleStatusConstants.Initialized, _testedClass.ModuleStatus);
            }
        }

        public class when_initializing_a_module_base_class_with_on_initialize_overridden_and_exception_is_thrown : Specification
        {
            const int Wait = 100;
            const int TestBeginWait = Wait/10;

            class ConcreateModule : ModuleBase
            {
                protected internal override void InternalInitialize()
                {
                    Thread.Sleep(Wait);
                    throw new SystemException();
                }
            }

            ModuleBase testedClass;
            private Thread _thread;

            protected override void Establish_That()
            {
                testedClass = new ConcreateModule();
            }

            protected override void Because_Of()
            {
                _thread = ThreadUtility.CreateThread(new ThreadStart(testedClass.Initialize), "");
            }

            [Fact]
            public void should_set_module_state_to_initializing()
            {
                Thread.Sleep(TestBeginWait);
                Assert.Equal<ModuleState>(ModuleState.Initializing, testedClass.ModuleState);
            }

            [Fact]
            public void should_eventually_set_state_to_health()
            {
                _thread.Join();
                Assert.Equal<ModuleState>(ModuleState.Error, testedClass.ModuleState);
            }

            [Fact]
            public void should_set_status_to_initialize_error()
            {
                _thread.Join();
                Assert.True(testedClass.ModuleStatus.StartsWith(ModuleStatusConstants.InitializeError));
            }
        }

        public class when_initializing_a_module_derived_from_module_base : Specification
        {
            public class ConcreteModule : ModuleBase
            {
                public bool OnInitializeCalled { get; set; }
                protected internal override void InternalInitialize()
                {
                    OnInitializeCalled = true;
                }
            }

            ConcreteModule testedClass;

            protected override void Establish_That()
            {
                testedClass = new ConcreteModule();
            }

            protected override void Because_Of()
            {
                testedClass.Initialize();
            }

            [Fact]
            public void should_call_on_initialize()
            {
                Assert.True(testedClass.OnInitializeCalled);
            }
        }

        public class when_disposing_of_a_module : Specification<ModuleBase>
        {
            protected override void Establish_That() { }

            protected override void Because_Of()
            {
                TestedClass.Dispose();
            }

            [Fact]
            public void should_set_Is_Disposed_to_true()
            {
                Assert.True(TestedClass.IsDisposed);
            }

            [Fact]
            public void should_set_module_state_to_disposed()
            {
                Assert.Equal<ModuleState>(ModuleState.Disposed, TestedClass.ModuleState);
            }
        }

        public class when_disposing_of_a_module_in_a_separate_thread : Specification
        {
            const int Wait = 100;
            const int TestBeginWait = Wait/10;

            class ConcreteModule : ModuleBase
            {
                protected internal override void InternalDispose()
                {
                    Thread.Sleep(Wait);
                }
            }

            ModuleBase testedClass;
            private Thread _thread;

            protected override void Establish_That()
            {
                testedClass = new ConcreteModule();
            }

            protected override void Because_Of()
            {
                _thread = ThreadUtility.CreateThread(testedClass.Dispose);
            }

            [Fact]
            public void should_set_is_disposing_to_true()
            {
                Thread.Sleep(TestBeginWait);
                Assert.Equal<bool>(true, testedClass.IsDisposing);
            }

            [Fact]
            public void should_set_is_disposing_to_false()
            {
                _thread.Join();
                Assert.Equal<bool>(false, testedClass.IsDisposing);
            }

            [Fact]
            public void should_set_is_disposed_to_true()
            {
                _thread.Join();
                Assert.Equal<bool>(true, testedClass.IsDisposed);
            }

            [Fact]
            public void should_set_module_state_to_disposed()
            {
                _thread.Join();
                Assert.Equal<ModuleState>(ModuleState.Disposed, testedClass.ModuleState);
            }
        }

        public class when_disposing_a_module_throws_an_exception : Specification
        {
            class ConcreteModule : ModuleBase
            {
                public bool InternalDisposeCalled { get; set; }
                protected internal override void InternalDispose()
                {
                    InternalDisposeCalled = true;
                    throw new SystemException();
                }
            }

            ModuleBase testedClass;

            protected override void Establish_That() 
            {
                testedClass = new ConcreteModule();
            }

            protected override void Because_Of()
            {
                testedClass.Dispose();
            }

            [Fact]
            public void should_set_internal_module_status()
            {
                Assert.Equal<string>(ModuleStatusConstants.DisposeError, testedClass.ModuleStatus);
            }
        }

        public class when_polling_a_module : Specification
        {
            class ConcreteModule : ModuleBase
            {
                public bool InternalPollCalled { get; set; }
                public override bool IsAutoPollingModule { get { return false; } }
                protected internal override void InternalPoll()
                {
                    InternalPollCalled = true;
                }
            }

            ModuleBase testedClass;

            protected override void Establish_That() 
            {
                testedClass = new ConcreteModule();
            }

            protected override void Because_Of()
            {
                testedClass.Poll();
            }

            [Fact]
            public void should_call_internal_poll()
            {
                Assert.True((testedClass as ConcreteModule).InternalPollCalled);
            }
        }

        public class when_polling_a_module_and_exceptions_are_thrown : Specification
        {
            class ConcreteModule : ModuleBase
            {
                public override bool IsAutoPollingModule { get { return false; } }
                protected internal override void InternalPoll()
                {
                    throw new ApplicationException();
                }
            }

            ModuleBase testedClass;

            protected override void Establish_That() 
            {
                testedClass = new ConcreteModule();
            }

            protected override void Because_Of()
            {
                testedClass.Poll();
            }

            [Fact]
            public void should_set_module_state_to_error()
            {
                Assert.Equal<ModuleState>(ModuleState.Error, testedClass.ModuleState);
            }

            [Fact]
            public void should_set_the_module_status_to_Error()
            {
                Assert.Equal(ModuleStatusConstants.Error, testedClass.ModuleStatus);
            }
        }

        public class when_polling_a_module_in_separate_threads : Specification
        {
            const int Wait = 100;
            const int TestBeginWait = Wait/10;

            class ConcreteModule : ModuleBase
            {
                public override bool IsAutoPollingModule { get { return false; } }
                protected internal override void InternalPoll()
                {
                    Thread.Sleep(Wait);
                }
            }

            ModuleBase testedClass;
            private Thread _thread;

            protected override void Establish_That() 
            {
                testedClass = new ConcreteModule();
            }

            protected override void Because_Of()
            {
                _thread = ThreadUtility.CreateThread(testedClass.Poll, "Poll Thread");
            }

            [Fact]
            public void should_set_is_polling_to_true()
            {
                Thread.Sleep(TestBeginWait);
                Assert.True(testedClass.IsPolling);
            }

            [Fact]
            public void should_set_is_polling_to_false()
            {
                _thread.Join();
                Assert.False(testedClass.IsPolling);
            }
        }

        public class when_a_module_is_auto_polling : Specification<ModuleBase>
        {
            int modulePolledCount = 0;
            protected override void Establish_That()
            {
                TestedClass.Expect(tc => tc.IsAutoPollingModule).CallOriginalMethod(OriginalCallOptions.NoExpectation);
                TestedClass.ModulePolled +=new EventHandler<ModuleEventArgs>(TestedClass_ModulePolled);
            }

            protected override void Because_Of()
            {
                TestedClass.Initialize();
            }

            void TestedClass_ModulePolled(object sender, ModuleEventArgs e)
            {
                modulePolledCount++;
            }

            [Fact]
            public void should_poll_at_least_once()
            {
                Thread.Sleep(2000);
                Assert.True(modulePolledCount > 0);
            }

            [Fact]
            public void should_stop_auto_polling_when_disposing_or_disposed()
            {
                Thread.Sleep(1000);
                var _beforeDispose = modulePolledCount;
                TestedClass.Dispose();
                var _afterDispose = modulePolledCount;
                Thread.Sleep(2000);

                Assert.True(_beforeDispose <= modulePolledCount);
                Assert.True(_afterDispose >= modulePolledCount);
            }
        }
    }
}
