using System;
using System.Collections;
using System.IO;
using NSubstitute.Core;
using NSubstitute.Tests.Stubs;
using NUnit.Framework;
using Shouldly;

namespace NSubstitute.Tests.Core
{
    [TestFixture]
    public class NameForSubstituteTests
    {
        [Test]
        public void GetNameFor_ReturnsQuestionMark_WhenTypeIsNull()
        {
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(null);

            result.ShouldBe("?");
        }

        [Test]
        public void GetNameFor_InterfaceSubstitution_ReturnsInterfaceName()
        {
            IDisposable substitute = Substitute.For<IDisposable>();
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(substitute);

            result.ShouldBe("IDisposable");
        }

        [Test]
        public void GetNameFor_AbstractClassSubstitution_ReturnsClassName()
        {
            Stream substitute = Substitute.For<Stream>();
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(substitute);

            result.ShouldBe("Stream");
        }

        [Test]
        public void GetNameFor_MultipleInterfaceSubstitution_ReturnsInterfaceName()
        {
            IEnumerable substitute = Substitute.For<IEnumerable, IDisposable>();
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(substitute);

            result.ShouldBe("IEnumerable");
        }

        [Test]
        public void GetNameFor_NonSubstitutedClass_ReturnsClassName()
        {
            var value = new TestClassTwo(Substitute.For<ITestInterfaceOne>());
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(value);

            result.ShouldBe("TestClassTwo");
        }
    }
}
