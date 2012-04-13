#if MSTest
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
using NUnit.Framework;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule.UnitTests.TestableClasses;

namespace nModule.UnitTests
{
    [TestFixture]
    public class ManagerModuleBaseTests
    {

        ManagerModuleBase<IModule> CreateStubManagerModuleBase()
        {
            return Rhino.Mocks.MockRepository.GenerateMock<ManagerModuleBase<IModule>>();
        }

        ConcreteManagerModuleBase<IModule> CreateConcreteManagerModuleBase()
        {
            return new ConcreteManagerModuleBase<IModule>();
        }

        [Test]
        public void Constructor_Void_Initializes()
        {
            ManagerModuleBase<IModule> target = CreateConcreteManagerModuleBase();
            Console.WriteLine("ManagerModuleBase Module Type - {0}", target.ModuleType);
            Assert.IsNotNull(target);
        }

    }
}
