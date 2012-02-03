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
		private class ConcreteErrorInitializeModule : Module
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

		private class ConcreteModule : Module
		{
			public ConcreteModule() : base() { }
			public ConcreteModule(string name) : base(name) { }

			public override string ModuleType
			{
				get { return "Concrete Module"; }
			}
		}

		private class ConcreteNonAutoPollingModule : Module
		{
			public ConcreteNonAutoPollingModule() : base() { }
			public ConcreteNonAutoPollingModule(string name) : base(name) { }

			public override string ModuleType { get { return "ConcreteNonAutoPollingModule"; } }
			public override bool IsAutoPollingModule { get { return false; } }
		}

		private class ConcreteSlowDisposingModule : Module
		{
			public ConcreteSlowDisposingModule() : base() { }
			public ConcreteSlowDisposingModule(string name) : base(name) { }

			public override string ModuleType
			{
				get { return "Concrete Module"; }
			}
		}

		internal static Module CreateModule()
		{
			return CreateStubModule("");
		}

		internal static Module CreateStubModule(string name)
		{
			Module module = Rhino.Mocks.MockRepository.GenerateStub<Module>(name);
			module.Stub(x => x.ModuleType).Return(String.Format("Mock {0}", name));
			return module;
		}

		internal static void TheadSleep()
		{
			System.Threading.Thread.Sleep(1000);
		}

		internal static Module CreateMockModule(string name)
		{
			Module module = Rhino.Mocks.MockRepository.GenerateMock<Module>(name);
			module.Stub(x => x.ModuleType).Return(String.Format("Mock{0}", name));
			return module;
		}

		internal static Module CreateSingletonStubModule(string name)
		{
			Module module = CreateStubModule(name);
			module.Stub(x => x.ModuleInstantiation).Return(ModuleInstantiation.Singleton);
			return module;
		}

		internal static Module CreateInstantiableStubModule(string name)
		{
			Module module = CreateStubModule(name);
			module.Stub(x => x.ModuleInstantiation).Return(ModuleInstantiation.Instantiable);
			return module;
		}

		internal static Module CreateSingletonAndInstantiableStubModule(string name)
		{
			Module module = CreateStubModule(name);
			module.Stub(x => x.ModuleInstantiation).Return(ModuleInstantiation.Instantiable | ModuleInstantiation.Singleton);
			return module;
		}

		internal static Module CreateInitializedErrorConcreteModule(string name)
		{
			return new ConcreteErrorInitializeModule(name);
		}

		internal static Module CreateConcreteBaseModule(string name)
		{
			return new ConcreteModule(name);
		}

		internal static Module CreateConcreteNonAutoPollingModule(string name)
		{
			return new ConcreteNonAutoPollingModule(name);
		}
	}
}
