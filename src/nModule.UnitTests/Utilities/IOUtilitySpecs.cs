using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule.UnitTests.Base;
using nModule.Utilities;
using Xunit;
using System.IO;

namespace nModule.UnitTests.Utilities
{
    public class IOUtilitySpecs
    {
        public class when_utilitizing_IO_utilities_for_combining_paths : Specification
        {
            const int pathLength = 10;
            string _path1;
            string _path2;
            private string _combinedPath;

            protected override void Establish_That()
            {
                _path1 = Random.NextString(pathLength);
                _path2 = Random.NextString(pathLength);
            }

            protected override void Because_Of()
            {
                _combinedPath = IOUtility.CombinePath(_path1, _path2);
            }

            [Fact]
            public void should_combime_the_paths()
            {
                Assert.Equal(_path1.Length + _path2.Length + 1, _combinedPath.Length);
                Assert.True(_combinedPath.Contains(_path1));
                Assert.True(_combinedPath.Contains(_path2));
                Assert.Equal(String.Format("{0}{1}{2}", _path1, System.IO.Path.DirectorySeparatorChar, _path2), _combinedPath);
            }
        }

        public class when_utilitizing_IO_utilities_for_combining_paths_and_second_value_starts_with_separator : Specification
        {
            const int pathLength = 10;
            string _path1;
            string _path2;
            private string _combinedPath;

            protected override void Establish_That()
            {
                _path1 = Random.NextString(pathLength);
                _path2 = "\\" + Random.NextString(pathLength);
            }

            protected override void Because_Of()
            {
                _combinedPath = IOUtility.CombinePath(_path1, _path2, true);
            }

            [Fact]
            public void should_combime_the_paths()
            {
                Assert.Equal(_path1.Length + _path2.Length, _combinedPath.Length);
                Assert.True(_combinedPath.Contains(_path1));
                Assert.True(_combinedPath.Contains(_path2));
                Assert.Equal(String.Format("{0}{1}{2}", _path1, System.IO.Path.DirectorySeparatorChar, _path2.Substring(1, _path2.Length - 1)), _combinedPath);
            }
        }

        public class when_utilizing_IO_utilities_to_combine_multiple_paths : Specification
        {
            const int pathLength = 10;
            IEnumerable<string> _paths;
            private string _combinedPath;

            protected override void Establish_That()
            {
                _paths = new[]
                {
                    Random.NextString(pathLength),
                    Random.NextString(pathLength),
                    Random.NextString(pathLength),
                    ""
                };
            }

            protected override void Because_Of()
            {
                _combinedPath = IOUtility.CombinePath(_paths);
            }

            [Fact]
            public void should_combime_the_paths()
            {
                Assert.True(_paths.All(path => _combinedPath.Contains(path)));
                Assert.Equal(_paths.Aggregate("", (current, path) =>
                {
                    if (String.IsNullOrEmpty(current))
                        return path;
                    if (String.IsNullOrEmpty(path))
                        return current;
                    return String.Format("{0}{1}{2}", current, System.IO.Path.DirectorySeparatorChar, path);
                }), _combinedPath);
            }
        }

        public class when_combining_environment_special_folder_paths : Specification
        {
            private Environment.SpecialFolder _specialFolder;
            private string _path;
            private string _result;

            protected override void Establish_That()
            {
                _specialFolder = Random.NextEnum<Environment.SpecialFolder>();
                _path = Random.NextString();
            }

            protected override void Because_Of()
            {
                _result = IOUtility.CombinePath(_specialFolder, _path);
            }

            [Fact]
            public void should_return_a_path()
            {
                Assert.NotNull(_result);
                Assert.NotEqual("", _result);
                Assert.True(_result.StartsWith(Environment.GetFolderPath(_specialFolder)));
            }
        }

        public class when_ensuring_a_path_exists : Specification
        {
            private Environment.SpecialFolder _specialFolder;
            private string _path;
            private string _combinedPath;
            private bool _result;
            protected override void Establish_That()
            {
                _specialFolder = Environment.SpecialFolder.ApplicationData;
                _path = Random.NextString();
                _combinedPath = IOUtility.CombinePath(_specialFolder, _path);
            }

            protected override void Because_Of()
            {
                _result = IOUtility.EnsurePathExists(_combinedPath);
            }

            [Fact]
            public void should_ensure_path_exists()
            {
                Assert.True(_result);
                Assert.True(Directory.Exists(_combinedPath));
                Directory.Delete(_combinedPath);
            }
        }

    }
}
