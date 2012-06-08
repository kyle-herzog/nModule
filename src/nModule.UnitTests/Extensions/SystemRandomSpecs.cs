using System;
using Xunit;
using nModule.UnitTests.Base;

namespace nModule.UnitTests.Extensions
{
    public class SystemRandomSpecs
    {
        public class when_using_next_string_extension_method_for_random : Specification
        {
            string _result;

            private Random TestedClass;

            protected override void Establish_That()
            {
                TestedClass = new Random();
            }

            protected override void Because_Of()
            {
                _result = TestedClass.NextString();
            }

            [Fact]
            public void should_return_a_string()
            {
                Assert.NotNull(_result);
                Assert.NotEqual("", _result);
            }

            [Fact]
            public void should_return_a_string_with_default_lenght()
            {
                Assert.Equal(SystemRandom.DefautRandomStringSize, _result.Length);
            }
        }

        public class when_using_next_string_extension_method_for_random_with_custom_length : Specification
        {
            string _result;

            private Random TestedClass;
            int _stringLength;

            protected override void Establish_That()
            {
                _stringLength = Random.Next(1, 10);
                TestedClass = new Random();
            }

            protected override void Because_Of()
            {
                _result = TestedClass.NextString(_stringLength);
            }

            [Fact]
            public void should_return_a_string()
            {
                Assert.NotNull(_result);
                Assert.NotEqual("", _result);
            }

            [Fact]
            public void should_return_a_string_with_default_lenght()
            {
                Assert.Equal(_stringLength, _result.Length);
            }
        }

        public class when_using_next_enum_extension_method_for_random : Specification
        {
            enum TestEnum { One, Two, Three }

            private System.Random TestedClass;
            private TestEnum _result;

            protected override void Establish_That()
            {
                TestedClass = new Random();
            }

            protected override void Because_Of()
            {
                _result = Random.NextEnum<TestEnum>();
            }

            [Fact]
            public void should_return_an_enum_value()
            {
                Assert.IsType<TestEnum>(_result);
            }

            [Fact]
            public void should_return_a_value_defined_within_test_enum()
            {
                Assert.True(Enum.IsDefined(typeof (TestEnum), _result));
            }
        }
        
        public class when_using_next_enum_extension_method_for_random_and_excluding_values : Specification
        {
            enum TestEnum { One, Two, Three }

            private System.Random TestedClass;
            private TestEnum _result;

            protected override void Establish_That()
            {
                TestedClass = new Random();
            }

            protected override void Because_Of()
            {
                _result = Random.NextEnum<TestEnum>(new[] {Random.NextEnum<TestEnum>()});
            }

            [Fact]
            public void should_never_return_excluded_enum_value()
            {
                Assert.IsType<TestEnum>(_result);
            }
        }

    }
}
