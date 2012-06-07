using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule.UnitTests.Base;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace nModule.UnitTests
{
    public class ManagerModuleBaseSpecs
    {
        public class ConcreteModule : ModuleBase { }

        public class when_creating_a_manager_module_base : Specification
        {
            private ManagerModuleBase<ConcreteModule> TestedClass;

            protected override void Establish_That()
            {
                TestedClass = Mocker.PartialMock<ManagerModuleBase<ConcreteModule>>();
                TestedClass.Expect(tc => tc.ModuleType).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            }

            protected override void Because_Of() { }

            [Fact]
            public void should_set_the_module_type()
            {
                Assert.Equal(typeof(ConcreteModule).Name + " Manager", TestedClass.ModuleType);
            }

            [Fact]
            public void should_retun_defaul_type_for_M_on_best_module()
            {
                Assert.Null(TestedClass.BestModule);
            }
        }
    }
}
