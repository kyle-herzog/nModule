#if MSTest
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
	public partial class ModuleTest
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

		internal void AssertModuleNameAreEqual(string name)
		{
			Module target = TestUtilities.CreateStubModule(name);
			Assert.AreEqual(target.ModuleName, name);
		}

		#endregion Test HelperMethods

		/// <summary>
		/// A Module test to ensure that the value of a Module's ModulePriority is assignable
		/// </summary>
		[Test]
		public void ModulePriority_CustomPriority_IsAssignable()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.AreEqual(0, target.ModulePriority);
			target.ModulePriority = 5000;
			Assert.AreEqual(5000, target.ModulePriority);
		}

		[Test]
		public void Initialize_Void_ModuleStateIsHealthy()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
			target.Initialize();
			Assert.AreEqual(ModuleState.Healthy, target.ModuleState);
		}

		[Test]
		public void Initialize_Void_ModuleStatusIsNotNullOrEmpty()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			target.Initialize();
            Assert.IsFalse(String.IsNullOrEmpty(target.ModuleStatus));
		}

		[Test]
		public void InternalInitialize_Void_ModuleStateIsErrored()
		{
			Module target = TestUtilities.CreateInitializedErrorConcreteModule("Test Target Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
			target.Initialize();
			Assert.AreEqual(ModuleState.Error, target.ModuleState);
		}

		[Test]
		public void Dispose_Void_IsDisposedIsTrue()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			target.Dispose();
			Assert.IsTrue(target.IsDisposed);			
		}

		[Test]
		public void Dispose_Void_OnDisposeCalled()
		{
			Nullable<bool> onDisposeCalled = null;
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			target.Stub(x => x.OnDispose()).Do((System.Action)delegate { onDisposeCalled = true; });
			target.Dispose();
			Assert.IsTrue(onDisposeCalled.HasValue);
			Assert.IsTrue(onDisposeCalled.Value);
		}

		[Test]
		public void Dispose_Void_OnDisposeSetsIsDisposing()
		{
			bool onDisposeCalled = false;
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			target.Stub(x => x.OnDispose()).Do((System.Action)delegate 
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
			Module target = TestUtilities.CreateStubModule("Test Target Module");
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
			Module target = TestUtilities.CreateStubModule("Test Target Module");
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
			Module target = TestUtilities.CreateConcreteBaseModule("Test Target Module");
			Assert.IsTrue(target.IsAutoPollingModule);
		}

		[Test]
		public void IsAutoPollingModule_Overridden_False()
		{
			Module target = TestUtilities.CreateMockModule("Test Target Module");
			target.Stub(x => x.IsAutoPollingModule).Return(false);
			Assert.IsFalse(target.IsAutoPollingModule);
		}

		[Test]
		public void IsAutoPollingModule_Overridden_True()
		{
			Module target = TestUtilities.CreateMockModule("Test Target Module");
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
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.AreNotEqual(target.ModuleId, 0);
		}

		/// <summary>
		/// A test for ModuleId ensurinng that the value is unique
		/// </summary>
		[Test]
		public void Constructor_String_ModuleIdIsUniquelyGenerated()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Module target1 = TestUtilities.CreateStubModule("Module1");
			Module target2 = TestUtilities.CreateStubModule("Module2");
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
			Module target = TestUtilities.CreateSingletonStubModule("Test Target Module");
			Assert.AreEqual(ModuleInstantiation.Singleton, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to ensure that the ModuleType is never Null or Empty
		/// </summary>
		[Test]
		public void Constructor_String_ModuleTypeIsSet()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.IsFalse(String.IsNullOrEmpty(target.ModuleType));
		}

		/// <summary>
		/// A Module test to ensure that the ModuleType is the expceted value
		/// </summary>
		[Test]
		public void Constructor_String_IsMockModule()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.AreEqual("Mock Test Target Module", target.ModuleType);
		}

		/// <summary>
		/// A Module test to ensure that the default value of a Module's ModulePriority is 0
		/// </summary>
		[Test]
		public void Constructor_String_ModulePriorityDefault()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.AreEqual(0, target.ModulePriority);
		}

		[Test]
		public void Constructor_String_ModuleStatusIsNotNullOrEmpty()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.IsFalse(String.IsNullOrEmpty(target.ModuleStatus));
		}

		[Test]
		public void Constructor_String_ModuleStateIsNotInitialized()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
		}

		[Test]
		public void Constructor_Void_IsDisposedIsFalse()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.IsFalse(target.IsDisposed);
		}

		[Test]
		public void Constructor_Void_IsDisposingIsFalse()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.IsFalse(target.IsDisposing);
		}

		[Test]
		public void Constructor_Void_IsPollingIsFalse()
		{
			Module target = TestUtilities.CreateStubModule("Test Target Module");
			Assert.IsFalse(target.IsPolling);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Instantiable
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_IsInstantiable()
		{
			Module target = TestUtilities.CreateInstantiableStubModule("Test Target Module");
			Assert.AreEqual(ModuleInstantiation.Instantiable, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Singleton
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_IsSingleton()
		{
			Module target = TestUtilities.CreateSingletonStubModule("Test Target Module");
			Assert.AreEqual(ModuleInstantiation.Singleton, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Instantiable and Singleton
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_InstantiableSingleton()
		{
			Module target = TestUtilities.CreateSingletonAndInstantiableStubModule("Test Target Module");
			Assert.IsTrue((target.ModuleInstantiation & ModuleInstantiation.Singleton) == ModuleInstantiation.Singleton);
			Assert.IsTrue((target.ModuleInstantiation & ModuleInstantiation.Instantiable) == ModuleInstantiation.Instantiable);
		}

		[Test]
		public void Constructor_PollTriggersModulePolled()
		{
			string moduleName = "Test Target Module";
			bool modulePolledCalled = false;
			Module target = TestUtilities.CreateConcreteNonAutoPollingModule(moduleName);
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
			Module target = TestUtilities.CreateConcreteBaseModule(moduleName);
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
			Module target = TestUtilities.CreateConcreteBaseModule(moduleName);
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
	}
}
