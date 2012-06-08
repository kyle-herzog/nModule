using System;
using Xunit;
using nModule.UnitTests.Base;

namespace nModule.UnitTests
{
    public class ApplicationInfoSpecs
    {
        public class when_accessing_aplication_info_for_the_first_time : Specification
        {
            private System.Reflection.Assembly _assembly;

            protected override void Establish_That() { }

            protected override void Because_Of()
            {
                _assembly = ApplicationInfo.Applicaition;
            }

            [Fact]
            public void should_discover_directory()
            {
                Assert.NotNull(ApplicationInfo.Directory);
            }

            [Fact]
            public void should_discover_application_assembly()
            {
                Assert.NotNull(ApplicationInfo.Applicaition);
            }

            [Fact]
            public void should_discover_application_name()
            {
                Assert.NotNull(ApplicationInfo.Name);
                Assert.Equal(_assembly.GetName().Name, ApplicationInfo.Name);
            }

            [Fact]
            public void should_discover_full_name()
            {
                Assert.NotNull(ApplicationInfo.FullName);
                Assert.Equal(_assembly.FullName, ApplicationInfo.FullName);
            }

            [Fact]
            public void should_discover_executable()
            {
                Assert.NotNull(ApplicationInfo.Executable);
                Assert.Equal(_assembly.Location, ApplicationInfo.Executable);
            }

            [Fact]
            public void should_generate_application_data()
            {
                Assert.NotNull(ApplicationInfo.ApplicationData);
                Assert.True(ApplicationInfo.ApplicationData.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
            }

            [Fact]
            public void should_generate_common_application_data()
            {
                Assert.NotNull(ApplicationInfo.CommonApplicationData);
                Assert.True(ApplicationInfo.CommonApplicationData.Contains(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)));
            }

            [Fact]
            public void should_discover_if_running_in_mono()
            {
                Assert.Equal(Type.GetType(ApplicationInfo.MonoRuntimeType) != null, ApplicationInfo.RunningInMono);
            }
        }
    }
}
