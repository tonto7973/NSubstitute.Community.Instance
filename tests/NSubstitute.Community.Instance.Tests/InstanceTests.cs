using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using NSubstitute.Core;
using NSubstitute.Tests.Stubs;
using NUnit.Framework;
using Shouldly;

namespace NSubstitute.Tests
{
    public class InstanceTests
    {
        [Test]
        public void InstanceOf_Throws_WhenTypeIsNull()
        {
            Action action = () => Instance.Of(null);

            action.ShouldThrow<ArgumentNullException>()
                .ParamName.ShouldBe("type");
        }

        [Test]
        public void InstanceOf_CreatesInstanceWithPublicConstructor_WhenNoArgumentsSpecified()
        {
            TestClassOne testObject = Instance.Of<TestClassOne>();

            testObject.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_CreatesInstance_WhenArgumentsInCorrectOrder()
        {
            ITestInterfaceOne arg1 = Substitute.For<ITestInterfaceOne>();
            IEnumerable arg2 = Substitute.For<IEnumerable>();
            Action<int> arg3 = _ => { };

            TestClassOne testObject = Instance.Of<TestClassOne>(arg1, arg2, arg3);

            testObject.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_CreatesInstance_WhenArgumentsInAnyOrder()
        {
            ITestInterfaceOne arg1 = Substitute.For<ITestInterfaceOne>();
            IEnumerable arg2 = Substitute.For<IEnumerable>();
            Action<int> arg3 = _ => { };

            TestClassOne testObject = Instance.Of<TestClassOne>(arg3, arg2, arg1);

            testObject.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_CreatesInstance_WhenArgumentIsMissing()
        {
            IEnumerable arg2 = Substitute.For<IEnumerable>();
            Action<int> arg3 = _ => { };

            TestClassOne testObject = Instance.Of<TestClassOne>(arg3, arg2);

            testObject.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_Throws_WhenArgumentDoesNotMatch()
        {
            IEqualityComparer arg = Substitute.For<IEqualityComparer>();

            Action action = () => Instance.Of<TestClassOne>(arg, 15, true);

            action.ShouldThrow<MissingMethodException>()
                .Message.ShouldBe("Cannot find accessible constructor on type 'NSubstitute.Tests.Stubs.TestClassOne' matching dependencies '[IEqualityComparer, int, bool]'.");
        }

        [Test]
        public void InstanceOf_DoesNotSubstituteDefaultParameters()
        {
            TestClassOne instance = Instance.Of<TestClassOne>(Substitute.For<ITestInterfaceOne>());

            instance.OnValue.ShouldBeNull();
            instance.Data.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_UsesConstructorWithLessArguments_WhenMultipleConstructorsDefined()
        {
            TestClassTwo instance = Instance.Of<TestClassTwo>();

            instance.TestInterface.ShouldNotBeNull();
            instance.Values.ShouldBeNull();
        }

        [Test]
        public void InstanceOf_UsesConstructorWithCorrectArguments_WhenMultipleConstructorsDefined()
        {
            TestClassTwo instance = Instance.Of<TestClassTwo>(new int[] { 13 });

            instance.TestInterface.ShouldNotBeNull();
            instance.Values.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_Throws_WhenClassConstructorThrows()
        {
            Action action = () => Instance.Of<TestClassThree>(Instance.Null<TestClassTwo>());
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void InstanceOf_Throws_WhenTypeIsInterface()
        {
            Action action = () => Instance.Of<IDisposable>();
            action.ShouldThrow<MemberAccessException>()
                .Message.ShouldBe("Cannot create an instance of type 'System.IDisposable' because it is an interface.");
        }

        [Test]
        public void InstanceOf_DoesNotThrow_WhenDependenciesAreNullArray()
        {
            Action action = () => Instance.Of<TestClassTwo>(null);
            action.ShouldNotThrow();
        }

        [Test]
        public void InstanceOf_CreatesInstanceUsingProtectedConstructor()
        {
            TestClassFour<bool> instance = Instance.Of<TestClassFour<bool>>();

            instance.Set.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_PassesNullToConstructor_WhenArgumentsPartial()
        {
            Action action = () => Instance.Of<TestClassTwo>(Instance.Null<ITestInterfaceOne>());

            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void InstanceOf_PassesNullToConstructor_WhenArgumentsSwapped()
        {
            Action action = () => Instance.Of<TestClassTwo>(
                Instance.Null<ICollection<int>>(),
                Instance.Null<ITestInterfaceOne>()
            );

            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void InstanceOf_CreatesAbstractClassWithProtectedConstructor()
        {
            TestBaseAbstractClassOne testInstance = Instance.Of<TestBaseAbstractClassOne>();

            testInstance.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_Throws_WhenDependencyCannotBeInstantiated()
        {
            Action action = () => Instance.Of<TestClassFive>();

            action.ShouldThrow<MissingMethodException>()
                .Message.ShouldBe("Cannot find accessible constructor on type 'NSubstitute.Tests.Stubs.TestSealedClass'.");
        }

        [Test]
        public void InstanceNull_ReturnsNullSubstitute()
        {
            INullValue nullSubstitute = Instance.Null<ITestInterfaceOne>();

            nullSubstitute.Type.ShouldBe(typeof(ITestInterfaceOne));
        }

        [Test]
        public void InstanceOf_CreatesSubstitutesForInterfaceAndAbstractClassAutomatically()
        {
            TestClassSix testInstance = Instance.Of<TestClassSix>();

            testInstance.AbstractClass.ShouldNotBeNull();
            testInstance.InterfaceOne.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_CreatesSubstitutesForProtectedInternal()
        {
            TestClassSeven testInstance = Instance.Of<TestClassSeven>();

            testInstance.TextWriter.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_Throws_WhenDependencyIsNull()
        {
            Action action = () => Instance.Of<TestClassSeven>(new object[] { null });

            var exception = action.ShouldThrow<ArgumentNullException>();
            exception.ParamName.ShouldBe("dependencies[0]");
            exception.Message.ShouldStartWith("Dependency cannot be null; Use Instance.Null instead.");
        }

        [Test]
        public void InstanceOf_JsonMediaTypeFormatter_DoesNotCauseStackOverflow()
        {
            JsonMediaTypeFormatter instance = Instance.Of<JsonMediaTypeFormatter>();

            instance.ShouldNotBeNull();
        }

        [Test]
        public void InstanceOf_Throws_WhenTypeConstructorReferencesSelf()
        {
            Action action = () => Instance.Of<TestStackClass>();

            action.ShouldThrow<NotSupportedException>()
                .Message.ShouldBe("Cannot create an instance of type 'NSubstitute.Tests.Stubs.TestStackClass' because its constructor '.ctor(TestStackClass,int)' depends on itself.");
        }
    }
}