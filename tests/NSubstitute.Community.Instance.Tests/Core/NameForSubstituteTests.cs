using System;
using System.Collections;
using System.Collections.Generic;
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

        [Test]
        public void GetNameFor_INullValue_ReturnsNull()
        {
            INullValue value = Instance.Null<IEnumerable>();
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(value);

            result.ShouldBe("null");
        }

        [Test]
        public void GetNameFor_Array_ResolvesArray()
        {
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(new bool[2]);

            result.ShouldBe("bool[]");
        }

        [Test]
        public void GetNameFor_GenericType_ResolvesGenerics()
        {
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(new List<double>());

            result.ShouldBe("List<double>");
        }

        [Test]
        public void GetNameFor_NestedGenericType_ResolvesGenerics()
        {
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNameFor(new List<NestedGenericTest<bool?, int>>());

            result.ShouldBe("List<NestedGenericTest<bool?,int>>");
        }

        private class NestedGenericTest<T1, T2> { }
    }
}
