using System;
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
    ///This is a test class for IModuleTest and is intended
    ///to contain all IModuleTest Unit Tests
    ///</summary>
	[TestFixture()]
	public class IModuleTest
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

		internal virtual IModule CreateIModule()
		{
			IModule target = MockRepository.GenerateStub<IModule>();
			target.Stub(x => x.ModuleName).Return("IModule");
			target.Stub(x => x.ModuleType).Return("MockIModule");
			target.Stub(x => x.ModuleId).Return(new Random().Next());
			target.Stub(x => x.ModuleInstantiation).Return(ModuleInstantiation.Singleton);
			target.Stub(x => x.ModuleStatus).Return("Instantiated");
			target.Stub(x => x.ModuleState).Return(ModuleState.NotInitialized);
			target.ModulePriority = 0;
			return target;
		}

		/// <summary>
		///A test for ModuleName
		///</summary>
		[Test]
		public void ModuleName_IsReturned()
		{
			IModule target = CreateIModule();
			Assert.AreEqual(target.ModuleName, "IModule");
		}

		/// <summary>
		///A test for ModuleInstantiation
		///</summary>
		[Test]
		public void ModuleInstantiation_IsReturned()
		{
			IModule target = CreateIModule();
			Assert.AreEqual(target.ModuleInstantiation, ModuleInstantiation.Singleton);
		}

		/// <summary>
		///A test for ModuleId
		///</summary>
		[Test]
		public void ModuleId_IsGreaterThanZero()
		{
			IModule target = CreateIModule();
			Assert.IsTrue(0 < target.ModuleId);
		}

		[Test]
		public void ModuleType_IsReturned()
		{
			IModule target = CreateIModule();
			Assert.AreEqual("MockIModule", target.ModuleType);
		}

		[Test]
		public void ModulePriority_IsReturned()
		{
			IModule target = CreateIModule();
			Assert.AreEqual(0, target.ModulePriority);
		}

		[Test]
		public void ModulePriority_IsAssignable()
		{
			IModule target = CreateIModule();
			Assert.AreEqual(0, target.ModulePriority);
			target.ModulePriority = 5000;
			Assert.AreEqual(5000, target.ModulePriority);
		}

		[Test]
		public void ModuleStatus_ReturnsValue()
		{
			IModule target = CreateIModule();
			Assert.IsFalse(String.IsNullOrEmpty(target.ModuleStatus));
		}

		[Test]
		public void ModuleState_ConstructorIsNotInitialized()
		{
			IModule target = CreateIModule();
			Assert.AreEqual(ModuleState.NotInitialized, target.ModuleState);
		}

		[Test]
		public void Dispose_DisposesModule()
		{
			IModule target = CreateIModule();
			target.Dispose();
			Assert.IsFalse(String.IsNullOrEmpty(target.ModuleName));
		}
	}
}
