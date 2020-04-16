using System;
using NUnit.Framework;
using NSubstitute.Core;
using Shouldly;

namespace NSubstitute.Tests.Core
{
    [TestFixture]
    public class NameForSubstituteExtensionsTests
    {
        [Test]
        public void GetNamesFor_Throws_WhenNameForSubstituteNull()
        {
            NameForSubstitute nameForSubstitute = null;

            Action action = () => nameForSubstitute.GetNamesFor(Array.Empty<object>());

            action.ShouldThrow<ArgumentNullException>()
                .ParamName.ShouldBe("nameForSubstitute");
        }

        [Test]
        public void GetNamesFor_ReturnsEmptyString_WhenArgumentsNull()
        {
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNamesFor(null);

            result.ShouldBe(string.Empty);
        }

        [Test]
        public void GetNamesFor_ReturnsEmptyString_WhenArgumentsEmpty()
        {
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNamesFor(Array.Empty<object>());

            result.ShouldBe(string.Empty);
        }

        [Test]
        public void GetNamesFor_ReturnsFormattedString_WhenArgumentsSet()
        {
            var arguments = new object[] {
                int.MinValue,
                DateTime.Now,
                (Action)(() => { }),
                Substitute.For<IDisposable>(),
                new Uri("http://test.io")
            };
            var nameForSubstitute = new NameForSubstitute();

            var result = nameForSubstitute.GetNamesFor(arguments);

            result.ShouldBe("Int32, DateTime, Action, IDisposable, Uri");
        }
    }
}
