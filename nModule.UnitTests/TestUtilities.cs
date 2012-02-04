using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule;
using Rhino.Mocks;

namespace nModule.UnitTests
{
	public static class TestUtilities
	{
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
	}
}
