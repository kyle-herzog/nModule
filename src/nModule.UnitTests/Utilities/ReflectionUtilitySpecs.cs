using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using nModule.UnitTests.Base;
using Xunit;
using nModule.Utilities;
using System.Reflection;

namespace nModule.UnitTests.Utilities
{
    public class ReflectionUtilitySpecs
    {
        public class when_using_the_reflection_utilities : Specification
        {
            private IDictionary<string, System.Reflection.Assembly> _results;

            protected override void Establish_That() { }

            protected override void Because_Of()
            {
                _results = ReflectionUtility.LoadAssemblies();
            }

            [Fact]
            public void should_return_only_assemblies_within_application_directory()
            {
                Assert.True(_results.All(pair => Path.GetDirectoryName(pair.Key) == ApplicationInfo.Directory));
            }

            [Fact]
            public void should_load_every_assembly()
            {
                Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                Assert.True(_results.All(pair => loadedAssemblies.Contains(pair.Value)));
            }
        }

        public class when_loading_types_assignable_to_a_type : Specification
        {
            private Type _assignableType;
            private Type[] _results;

            protected override void Establish_That()
            {
                _assignableType = typeof(object);
            }

            protected override void Because_Of()
            {
                _results = ReflectionUtility.LoadAssignableType(_assignableType);
            }

            [Fact]
            public void should_return_any_type()
            {
                Assert.True(_results.Length > 0);
            }
        }

    }
}
