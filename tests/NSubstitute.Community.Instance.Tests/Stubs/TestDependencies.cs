using System;
using System.Collections.Generic;

namespace NSubstitute.Tests.Stubs
{
    public class TestDependencies(ITestInterfaceOne dependencyOne, IEnumerable<bool> dependencyTwo, ICloneable dependencyThree)
    {
        internal readonly ITestInterfaceOne _dependencyOne = dependencyOne ?? throw new ArgumentNullException(nameof(dependencyOne));
        internal readonly IEnumerable<bool> _dependencyTwo = dependencyTwo ?? throw new ArgumentNullException(nameof(dependencyTwo));
        internal readonly ICloneable _dependencyThree = dependencyThree;
    }
}
