using System;
using System.Collections;
using NSubstitute.Tests.Stubs;
using NUnit.Framework;
using Shouldly;

namespace NSubstitute.Tests
{
    public class InstanceTests
    {
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
                .Message.ShouldBe("Cannot find a constructor on type 'NSubstitute.Tests.Stubs.TestClassOne' matching dependencies '[IEqualityComparer, Int32, Boolean]'.");
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
            Action action = () => Instance.Of<TestClassThree>();
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void InstanceOf_Throws_WhenTypeIsInterface()
        {
            Action action = () => Instance.Of<IDisposable>();
            action.ShouldThrow<MemberAccessException>()
                .Message.ShouldBe("Cannot create an instance of 'System.IDisposable' because it is an interface.");
        }

        [Test]
        public void InstanceOf_Throws_WhenTypeIsAbstract()
        {
            Action action = () => Instance.Of<TestBaseClassOne>();
            action.ShouldThrow<MemberAccessException>()
                .Message.ShouldBe("Cannot create an instance of 'NSubstitute.Tests.Stubs.TestBaseClassOne' because it is an abstract class.");
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
    }
}