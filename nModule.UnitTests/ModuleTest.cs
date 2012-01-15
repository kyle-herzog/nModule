using System;
using System.Linq;
using nModule;
using Rhino.Mocks;
#if MSTest
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace nModule.UnitTests
{
	/// <summary>
	///This is a test class for ModuleTest and is intended
	///to contain all ModuleTest Unit Tests
	///</summary>
	[TestFixture()]
	public class ModuleTest
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
			AssertModuleNameAreEqual("Module");
		}

		/// <summary>
		/// A test for ModuleId ensuring that the value is NOT 0, default value.
		/// </summary>
		[Test]
		public void Constructor_String_ModuleIdIsGenerated()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.AreNotEqual(target.ModuleId, 0);
		}

		/// <summary>
		/// A test for ModuleId ensurinng that the value is unique
		/// </summary>
		[Test]
		public void Constructor_String_ModuleIdIsUniquelyGenerated()
		{
			Module target = TestUtilities.CreateStubModule("Module");
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
#if MSTest
        [Ignore]
#else
        //[Ignore("This test doesn't work because when RhinoMocks stub or mock the object it breaks the return type/value of the virtual ModuleInstantiation")]
#endif
        public void Constructor_String_DefaultModuleInstantiationIsCorrect()
		{
			Module target = TestUtilities.CreateSingletonStubModule("Module");
			Assert.AreEqual(ModuleInstantiation.Singleton, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Instantiable
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_IsInstantiable()
		{
			Module target = TestUtilities.CreateInstantiableStubModule("Module");
			Assert.AreEqual(ModuleInstantiation.Instantiable, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Singleton
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_IsSingleton()
		{
			Module target = TestUtilities.CreateSingletonStubModule("Module");
			Assert.AreEqual(ModuleInstantiation.Singleton, target.ModuleInstantiation);
		}

		/// <summary>
		/// A Module test to test the overriding functionality of the ModuleInstantiation property to return Instantiable and Singleton
		/// </summary>
		[Test]
		public void ModuleInstantiation_Override_InstantiableSingleton()
		{
			Module target = TestUtilities.CreateSingletonAndInstantiableStubModule("Module");
			Assert.IsTrue((target.ModuleInstantiation & ModuleInstantiation.Singleton) == ModuleInstantiation.Singleton);
			Assert.IsTrue((target.ModuleInstantiation & ModuleInstantiation.Instantiable) == ModuleInstantiation.Instantiable);
		}

		/// <summary>
		/// A Module test to ensure that the ModuleType is never Null or Empty
		/// </summary>
		[Test]
		public void Constructor_String_ModuleTypeIsSet()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.IsFalse(String.IsNullOrEmpty(target.ModuleType));
		}

		/// <summary>
		/// A Module test to ensure that the ModuleType is the expceted value
		/// </summary>
		[Test]
		public void Constructor_String_IsMockModule()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.AreEqual("MockModule", target.ModuleType);
		}

		/// <summary>
		/// A Module test to ensure that the default value of a Module's ModulePriority is 0
		/// </summary>
		[Test]
		public void Constructor_String_ModulePriorityDefault()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.AreEqual(0, target.ModulePriority);
		}

		/// <summary>
		/// A Module test to ensure that the value of a Module's ModulePriority is assignable
		/// </summary>
		[Test]
		public void ModulePriority_CustomPriority_IsAssignable()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.AreEqual(0, target.ModulePriority);
			target.ModulePriority = 5000;
			Assert.AreEqual(5000, target.ModulePriority);
		}

		[Test]
		public void Constructor_String_ModuleStatusIsNotNullOrEmpty()
		{
			Module target = TestUtilities.CreateStubModule("Module");
            Assert.IsFalse(String.IsNullOrEmpty(target.ModuleStatus));
		}

		[Test]
		public void Constructor_String_ModuleStateIsNotInitialized()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
		}

		[Test]
		public void Initialize_Void_ModuleStateIsHealthy()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
			target.Initialize();
			Assert.AreEqual(ModuleState.Healthy, target.ModuleState);
		}

		[Test]
		public void Initialize_Void_ModuleStatusIsNotNullOrEmpty()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			target.Initialize();
            Assert.IsFalse(String.IsNullOrEmpty(target.ModuleStatus));
		}

		[Test]
		public void InternalInitialize_Void_ModuleStateIsErrored()
		{
			Module target = TestUtilities.CreateInitializedErrorConcreteModule("Module");
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
			target.Initialize();
			Assert.AreEqual(ModuleState.Error, target.ModuleState);
		}

		[Test]
		public void Constructor_Void_IsDisposedIsFalse()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.IsFalse(target.IsDisposed);
		}

		[Test]
		public void Dispose_Void_IsDisposedIsTrue()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			target.Dispose();
			Assert.IsTrue(target.IsDisposed);			
		}

		[Test]
		public void Constructor_Void_IsDisposingIsFalse()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			Assert.IsFalse(target.IsDisposing);
		}
		
		[Test]
		public void Dispose_Void_OnDisposeCalled()
		{
			Nullable<bool> onDisposeCalled = null;
			Module target = TestUtilities.CreateStubModule("Module");
			//target.Stub(x => x.Dispose()).Do((System.Action)delegate { onDisposeCalled = false; });
			target.Stub(x => x.OnDispose()).Do((System.Action)delegate { onDisposeCalled = true; });
			target.Dispose();
			Assert.IsTrue(onDisposeCalled.HasValue);
			Assert.IsTrue(onDisposeCalled.Value);
		}

		[Test]
		public void Dispose_Void_OnDisposeSetsIsDisposing()
		{
			bool onDisposeCalled = false;
			Module target = TestUtilities.CreateStubModule("Module");
			target.Stub(x => x.OnDispose()).Do((System.Threading.ThreadStart)delegate { onDisposeCalled = true; System.Threading.Thread.Sleep(1000); });
			System.Threading.Thread thread = new System.Threading.Thread((System.Threading.ThreadStart)delegate
				{
					target.Dispose();
				}
			);
			thread.Start();
			System.Threading.Thread.Sleep(100);
			Assert.IsTrue(onDisposeCalled);
			Assert.IsTrue(target.IsDisposing);
			System.Threading.Thread.Sleep(1000);
			Assert.IsFalse(target.IsDisposing);
			Assert.IsTrue(target.IsDisposed);
		}

		[Test]
		public void Poll_Void_ModuleStatusUpdated()
		{
			Module target = TestUtilities.CreateStubModule("Module");
			
		}

	}
}
