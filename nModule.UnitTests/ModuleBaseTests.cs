﻿#if MSTest
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
using NUnit.Framework;
#endif
using System;
using System.Threading;
using System.Diagnostics;
using nModule;
using Rhino.Mocks;
using System.Collections.Generic;

namespace nModule.UnitTests
{
	/// <summary>
	///This is a test class for ModuleTest and is intended
	///to contain all ModuleTest Unit Tests
	///</summary>
	[TestFixture()]
	public partial class ModuleBaseTests
	{
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region TestHelperMethods

        private class ConcreteErrorInitializeModule : ModuleBase
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

        private class ConcreteModule : ModuleBase
        {
            public ConcreteModule() : base() { }
            public ConcreteModule(string name) : base(name) { }

            public override string ModuleType
            {
                get { return "Concrete Module"; }
            }
        }

        private class ConcreteNonAutoPollingModule : ModuleBase
        {
            public ConcreteNonAutoPollingModule() : base() { }
            public ConcreteNonAutoPollingModule(string name) : base(name) { }

            public override string ModuleType { get { return "ConcreteNonAutoPollingModule"; } }
            public override bool IsAutoPollingModule { get { return false; } }
        }

        private class ConcreteSlowDisposingModule : ModuleBase
        {
            public ConcreteSlowDisposingModule() : base() { }
            public ConcreteSlowDisposingModule(string name) : base(name) { }

            public override string ModuleType
            {
                get { return "Concrete Module"; }
            }
        }

        internal static ModuleBase CreateModule()
        {
            return CreateStubModule("");
        }

        internal static ModuleBase CreateStubModule(string name)
        {
            ModuleBase module = Rhino.Mocks.MockRepository.GenerateStub<ModuleBase>(name);
            module.Stub(x => x.ModuleType).Return(String.Format("Mock {0}", name));
            return module;
        }

        internal static void TheadSleep()
        {
            System.Threading.Thread.Sleep(1000);
        }

        internal static ModuleBase CreateMockModule(string name)
        {
            ModuleBase module = Rhino.Mocks.MockRepository.GenerateMock<ModuleBase>(name);
            module.Stub(x => x.ModuleType).Return(String.Format("Mock{0}", name));
            return module;
        }

        internal static ModuleBase CreateSingletonStubModule(string name)
        {
            ModuleBase module = CreateStubModule(name);
            module.Stub(x => x.ModuleInstantiation).Return(ModuleInstantiation.Singleton);
            return module;
        }

        internal static ModuleBase CreateInstantiableStubModule(string name)
        {
            ModuleBase module = CreateStubModule(name);
            module.Stub(x => x.ModuleInstantiation).Return(ModuleInstantiation.Instantiable);
            return module;
        }

        internal static ModuleBase CreateSingletonAndInstantiableStubModule(string name)
        {
            ModuleBase module = CreateStubModule(name);
            module.Stub(x => x.ModuleInstantiation).Return(ModuleInstantiation.Instantiable | ModuleInstantiation.Singleton);
            return module;
        }

        internal static ModuleBase CreateInitializedErrorConcreteModule(string name)
        {
            return new ConcreteErrorInitializeModule(name);
        }

        internal static ModuleBase CreateConcreteBaseModule(string name)
        {
            return new ConcreteModule(name);
        }

        internal static ModuleBase CreateConcreteNonAutoPollingModule(string name)
        {
            return new ConcreteNonAutoPollingModule(name);
        }

		internal void AssertModuleNameAreEqual(string name)
		{
			ModuleBase target = CreateStubModule(name);
			Assert.AreEqual(target.ModuleName, name);
		}

		#endregion Test HelperMethods

		/// <summary>
		/// A Module test to ensure that the value of a Module's ModulePriority is assignable
		/// </summary>
		[Test]
		public void ModulePriority_CustomPriority_IsAssignable()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.AreEqual(0, target.ModulePriority);
			target.ModulePriority = 5000;
			Assert.AreEqual(5000, target.ModulePriority);
		}

		[Test]
		public void Initialize_Void_ModuleStateIsHealthy()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
			target.Initialize();
			Assert.AreEqual(ModuleState.Healthy, target.ModuleState);
		}

		[Test]
		public void Initialize_Void_ModuleStatusIsNotNullOrEmpty()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			target.Initialize();
            Assert.IsFalse(String.IsNullOrEmpty(target.ModuleStatus));
		}

		[Test]
		public void InternalInitialize_Void_ModuleStateIsErrored()
		{
			ModuleBase target = CreateInitializedErrorConcreteModule("Test Target Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
			target.Initialize();
			Assert.AreEqual(ModuleState.Error, target.ModuleState);
		}

		[Test]
		public void Dispose_Void_IsDisposedIsTrue()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			target.Dispose();
			Assert.IsTrue(target.IsDisposed);			
		}

		[Test]
		public void Dispose_Void_OnDisposeCalled()
		{
			Nullable<bool> onDisposeCalled = null;
			ModuleBase target = CreateStubModule("Test Target Module");
			target.Stub(x => x.InternalDispose()).Do((System.Action)delegate { onDisposeCalled = true; });
			target.Dispose();
			Assert.IsTrue(onDisposeCalled.HasValue);
			Assert.IsTrue(onDisposeCalled.Value);
		}

		[Test]
		public void Dispose_Void_OnDisposeSetsIsDisposing()
		{
			bool onDisposeCalled = false;
			ModuleBase target = CreateStubModule("Test Target Module");
			target.Stub(x => x.InternalDispose()).Do((System.Action)delegate 
			    { 
					onDisposeCalled = true; 
					Thread.Sleep(1000); 
				}
			);
			Thread thread = new Thread((ThreadStart)delegate
				{
					target.Dispose();
				}
			);
			thread.Start();
			Thread.Sleep(100);
			Assert.IsTrue(onDisposeCalled);
			Assert.IsTrue(target.IsDisposing);
			Thread.Sleep(1000);
			Assert.IsFalse(target.IsDisposing);
			Assert.IsTrue(target.IsDisposed);
		}
		[Test]
		public void Poll_Void_InternalPollCalled()
		{
			Nullable<bool> InternalPollCalled = null;
			ModuleBase target = CreateStubModule("Test Target Module");
			target.Stub(x => x.InternalPoll()).Do((System.Action)delegate
				{
					InternalPollCalled = true;
				}
			);
			target.Poll();
			Assert.IsTrue(InternalPollCalled.HasValue, "InternalPollCalled does not have a value set");
			Assert.IsTrue(InternalPollCalled.Value);
		}

		[Test]
		public void Poll_Void_PollSetsIsPolling()
		{
			Nullable<bool> InternalPollCalled = null;
			ModuleBase target = CreateStubModule("Test Target Module");
			target.Stub(x => x.InternalPoll()).Do((System.Action)delegate
				{
					InternalPollCalled = true;
					System.Threading.Thread.Sleep(1000);
				}
			);
			Thread thread = new Thread((ThreadStart)delegate
				{
					target.Poll();
				}
			);
			thread.Start();
			System.Threading.Thread.Sleep(100);
			Assert.IsTrue(InternalPollCalled.HasValue);
			Assert.IsTrue(InternalPollCalled.Value);
			Assert.IsTrue(target.IsPolling);
			System.Threading.Thread.Sleep(1000);
			Assert.IsFalse(target.IsPolling);
		}

		[Test]
		public void IsAutoPollingModule_True()
		{
			ModuleBase target = CreateConcreteBaseModule("Test Target Module");
			Assert.IsTrue(target.IsAutoPollingModule);
		}

		[Test]
		public void IsAutoPollingModule_Overridden_False()
		{
			ModuleBase target = CreateMockModule("Test Target Module");
			target.Stub(x => x.IsAutoPollingModule).Return(false);
			Assert.IsFalse(target.IsAutoPollingModule);
		}

		[Test]
		public void IsAutoPollingModule_Overridden_True()
		{
			ModuleBase target = CreateMockModule("Test Target Module");
			target.Stub(x => x.IsAutoPollingModule).Return(true);
			Assert.IsTrue(target.IsAutoPollingModule);
		}

		/// <summary>
		///A test for ModuleName being set to empty an string 
		///</summary>
		[Test]
		public void Constructor_EmptyString_ModuleNameIsEmptyString()
		{
			AssertModuleNameAreEqual("");
		}

		/// <summary>
		///A test for ModuleName being the value set at instantiation time
		///</summary>
		[Test]
		public void Constructor_String_ModuleNameIsStringValue()
		{
			AssertModuleNameAreEqual("Test Target Module");
		}

		/// <summary>
		/// A test for ModuleId ensuring that the value is NOT 0, default value.
		/// </summary>
		[Test]
		public void Constructor_String_ModuleIdIsGenerated()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.AreNotEqual(target.ModuleId, 0);
		}

		/// <summary>
		/// A test for ModuleId ensurinng that the value is unique
		/// </summary>
		[Test]
		public void Constructor_String_ModuleIdIsUniquelyGenerated()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			ModuleBase target1 = CreateStubModule("Module1");
			ModuleBase target2 = CreateStubModule("Module2");
			Assert.AreNotEqual(target.ModuleId, target1.ModuleId);
			Assert.AreNotEqual(target.ModuleId, target2.ModuleId);
			Assert.AreNotEqual(target1.ModuleId, target2.ModuleId);
		}

		/// <summary>
		/// A Module test to ensure the base method returns Singleton for its ModuleInstantiation
		/// </summary>
		[Test]
		public void Constructor_String_DefaultModuleInstantiationIsCorrect()
		{
			ModuleBase target = CreateSingletonStubModule("Test Target Module");
			Assert.AreEqual(ModuleInstantiation.Singleton, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to ensure that the ModuleType is never Null or Empty
		/// </summary>
		[Test]
		public void Constructor_String_ModuleTypeIsSet()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.IsFalse(String.IsNullOrEmpty(target.ModuleType));
		}

		/// <summary>
		/// A Module test to ensure that the ModuleType is the expceted value
		/// </summary>
		[Test]
		public void Constructor_String_IsMockModule()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.AreEqual("Mock Test Target Module", target.ModuleType);
		}

		/// <summary>
		/// A Module test to ensure that the default value of a Module's ModulePriority is 0
		/// </summary>
		[Test]
		public void Constructor_String_ModulePriorityDefault()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.AreEqual(0, target.ModulePriority);
		}

		[Test]
		public void Constructor_String_ModuleStatusIsNotNullOrEmpty()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.IsFalse(String.IsNullOrEmpty(target.ModuleStatus));
		}

		[Test]
		public void Constructor_String_ModuleStateIsNotInitialized()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
		}

		[Test]
		public void Constructor_Void_IsDisposedIsFalse()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.IsFalse(target.IsDisposed);
		}

		[Test]
		public void Constructor_Void_IsDisposingIsFalse()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.IsFalse(target.IsDisposing);
		}

		[Test]
		public void Constructor_Void_IsPollingIsFalse()
		{
			ModuleBase target = CreateStubModule("Test Target Module");
			Assert.IsFalse(target.IsPolling);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Instantiable
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_IsInstantiable()
		{
			ModuleBase target = CreateInstantiableStubModule("Test Target Module");
			Assert.AreEqual(ModuleInstantiation.Instantiable, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Singleton
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_IsSingleton()
		{
			ModuleBase target = CreateSingletonStubModule("Test Target Module");
			Assert.AreEqual(ModuleInstantiation.Singleton, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Instantiable and Singleton
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_InstantiableSingleton()
		{
			ModuleBase target = CreateSingletonAndInstantiableStubModule("Test Target Module");
			Assert.IsTrue((target.ModuleInstantiation & ModuleInstantiation.Singleton) == ModuleInstantiation.Singleton);
			Assert.IsTrue((target.ModuleInstantiation & ModuleInstantiation.Instantiable) == ModuleInstantiation.Instantiable);
		}

		[Test]
		public void Constructor_PollTriggersModulePolled()
		{
			string moduleName = "Test Target Module";
			bool modulePolledCalled = false;
			ModuleBase target = CreateConcreteNonAutoPollingModule(moduleName);
			target.ModulePolled += new EventHandler<ModuleEventArgs>(
				(object sender, ModuleEventArgs e) =>
				{
					Assert.IsFalse(String.IsNullOrEmpty(e.ModuleName));
					Assert.IsTrue(e.ModuleName == moduleName);
					Assert.IsTrue(e.ModuleThreadId == Thread.CurrentThread.ManagedThreadId);
					modulePolledCalled = true;
				}
			);
			target.Poll();
			int tries = 0;
			while (!modulePolledCalled || tries <= 10)
			{
				Thread.Sleep(100); tries++;
			}
			Assert.IsTrue(modulePolledCalled);
		}

		[Test]
		public void Constructor_IsAutoPollingModule_CreatesSeparatePollingThread()
		{
			string moduleName = "Test Target Module";
			bool modulePolledCalled = false;
			int modulePolledThreadId = 0;
			ModuleBase target = CreateConcreteBaseModule(moduleName);
			target.ModulePolled += new EventHandler<ModuleEventArgs>(
				(object sender, ModuleEventArgs e) =>
				{
					Assert.IsFalse(String.IsNullOrEmpty(e.ModuleName));
					Assert.IsTrue(e.ModuleName == moduleName);
					Assert.IsTrue(e.ModuleThreadId == Thread.CurrentThread.ManagedThreadId);
					modulePolledThreadId = e.ModuleThreadId;
					modulePolledCalled = true;
				}
			);
			target.Initialize();
			int tries = 0;
			while (!modulePolledCalled && tries <= 10)
			{
				Thread.Sleep(100); tries++;
			}
			target.Dispose();
			Assert.AreNotEqual(modulePolledThreadId, Thread.CurrentThread.ManagedThreadId);
			Assert.IsTrue(modulePolledCalled);
		}

		[Test]
		public void ModulePollFrequency_500Millis_PollsTimely()
		{
			string moduleName = "Test Target Module";
			int modulePolledThreadId = 0;
			int testMethodThreadId = Thread.CurrentThread.ManagedThreadId;
			List<double> pollTimes = new List<double>();
			ModuleBase target = CreateConcreteBaseModule(moduleName);
			target.ModuleAutoPollFrequency = 500;
			Stopwatch pollStopWatch = new Stopwatch();
			int tries = 0;
			pollStopWatch.Start();
			target.ModulePolled += new EventHandler<ModuleEventArgs>(
				(object sender, ModuleEventArgs e) =>
				{
					modulePolledThreadId = Thread.CurrentThread.ManagedThreadId;
					pollStopWatch.Stop();
					pollTimes.Add(pollStopWatch.Elapsed.TotalMilliseconds);
					Assert.IsFalse(String.IsNullOrEmpty(e.ModuleName));
					Assert.IsTrue(e.ModuleName == moduleName);
					Assert.AreNotEqual(testMethodThreadId, Thread.CurrentThread.ManagedThreadId);
					Assert.IsTrue(e.ModuleThreadId == modulePolledThreadId);
					tries++;
					pollStopWatch.Reset();
					pollStopWatch.Start();
				}
			);
			target.Initialize();
			while (tries <= 3)
			{
				Thread.Sleep(100);
			}
			target.Dispose();
			Assert.AreNotEqual(modulePolledThreadId, testMethodThreadId);
			foreach (double pollTime in pollTimes)
			{

				Assert.IsTrue(pollTime >= 490, "Poll frequency was outside of accepted lower bounds - {0}", pollTime);
				Assert.IsTrue(pollTime <= 510, "Poll frequency was outside of accepted upper bounds - {0}", pollTime);
			}
		}

        [Test]
        public void Constructor_IsAutoPollingModule_DoesNotCreatePollingThread()
        {
            string moduleName = "Test Target Module";
            int modulePolledThreadId = 0;
            bool modulePolledCalled = false;
            int testMethodThreadId = Thread.CurrentThread.ManagedThreadId;
            ModuleBase target = CreateConcreteNonAutoPollingModule(moduleName);
            Stopwatch pollStopWatch = new Stopwatch();
            int tries = 0;
            target.ModulePolled += new EventHandler<ModuleEventArgs>(
                (object sender, ModuleEventArgs e) =>
                {
                    modulePolledCalled = true;
                    modulePolledThreadId = Thread.CurrentThread.ManagedThreadId;
                }
            );
            target.Initialize();
            target.Poll();
            while (!modulePolledCalled && tries <= 5)
            {
                Thread.Sleep(100);
                tries++;
            }
            target.Dispose();
            Assert.IsTrue(modulePolledCalled);
            Assert.AreEqual(modulePolledThreadId, testMethodThreadId);
        }

	}
}
