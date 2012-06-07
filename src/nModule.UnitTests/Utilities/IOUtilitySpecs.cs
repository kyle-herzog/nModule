using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nModule.UnitTests.Base;
using nModule.Utilities;
using Xunit;

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


    }
}
