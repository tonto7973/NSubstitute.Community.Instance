using System;
using System.Collections.Generic;

namespace NSubstitute.Tests.Stubs
{
    public class TestDependencies
    {
        private readonly ITestInterfaceOne _dependencyOne;
        private readonly IEnumerable<bool> _dependencyTwo;
        private readonly ICloneable _dependencyThree;

        public TestDependencies(ITestInterfaceOne dependencyOne, IEnumerable<bool> dependencyTwo, ICloneable dependencyThree)
        {
            _dependencyOne = dependencyOne ?? throw new ArgumentNullException(nameof(dependencyOne));
            _dependencyTwo = dependencyTwo ?? throw new ArgumentNullException(nameof(dependencyTwo));
            _dependencyThree = dependencyThree;
        }
    }
}
