using LinkLynx.Wiring.DI;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace LinkLynx.Tests.UnitTests.Dependency_Injection.Scenarios
{
    [TestFixture]
    public class GeneralTests
    {
        [Test, Category("Unit Tests")]
        public void BasicUseTest()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ITestClass, TestClass>();

            ServiceProvider provider = services.BuildServiceProvider();

            ITestClass instance = provider.GetRequired<ITestClass>();

            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.TypeOf<TestClass>());
        }

        [Test, Category("Unit Tests")]
        public void SingletonReturnsSameInstance()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ITestClass, TestClass>();

            ServiceProvider provider = services.BuildServiceProvider();

            ITestClass instanceOne = provider.GetRequired<ITestClass>();
            ITestClass instanceTwo = provider.GetRequired<ITestClass>();

            Assert.That(instanceOne, Is.SameAs(instanceTwo));
        }

        [Test, Category("Unit Tests")]
        public void TransientReturnsNewInstances()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<ITestClass, TestClass>();

            ServiceProvider provider = services.BuildServiceProvider();

            ITestClass instanceOne = provider.GetRequired<ITestClass>();
            ITestClass instanceTwo = provider.GetRequired<ITestClass>();

            Assert.That(instanceOne, Is.Not.SameAs(instanceTwo));
        }

        [Test, Category("Unit Tests")]
        public void ResolvesDependenciesInConstructor()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IAnimal, Dog>();
            services.AddSingleton<Cage, Cage>();

            ServiceProvider provider = services.BuildServiceProvider();

            Cage cage = provider.GetRequired<Cage>();

            IAnimal animal = cage.GetAnimal();

            Assert.That(cage.GetAnimal(), Is.SameAs(animal));
            Assert.That(animal, Is.TypeOf<Dog>());
        }

        [Test, Category("Unit Tests")]
        public void ResolvingUnregisteredServiceThrows()
        {
            ServiceCollection services = new ServiceCollection();

            ServiceProvider provider = services.BuildServiceProvider();

            Assert.Throws<InvalidOperationException>(() => provider.GetRequired<ITestClass>());
        }

        [Test, Category("Unit Tests")]
        public void CreateInstanceFromDescriptorShouldInjectDependencies()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IDepA, DepA>();
            services.AddTransient<IDepB, DepB>();
            services.AddTransient<ILargeTestClass, LargeTestClass>(); // TestClass has (IDepA, IDepB)

            ServiceProvider provider = services.BuildServiceProvider();

            ILargeTestClass instance = provider.GetRequired<ILargeTestClass>();
            LargeTestClass? typed = instance as LargeTestClass;

            Assert.That(instance, Is.Not.Null);
            Assert.That(typed!.A, Is.Not.Null);
            Assert.That(typed.B, Is.Not.Null);
        }

        [Test, Category("Unit Tests")]
        public void CreateInstanceFromDescriptor_ShouldThrowWhenDependencyMissing()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<ILargeTestClass, LargeTestClass>();

            ServiceProvider provider = services.BuildServiceProvider();

            Exception? ex = Assert.Throws<InvalidOperationException>(
                () => provider.GetRequired<ILargeTestClass>()
            );

            StringAssert.Contains("Missing dependency", ex!.Message);
        }
    }

    public class TestClass : ITestClass {}

    public interface ITestClass { }


    public class Cage(IAnimal animal) { public IAnimal GetAnimal() { return animal; } }

    public class Dog : IAnimal {}

    public interface IAnimal {}


    public interface IDepA { }
    public class DepA : IDepA { }

    public interface IDepB { }
    public class DepB : IDepB { }

    public interface ILargeTestClass { }
    public class LargeTestClass : ILargeTestClass
    {
        public IDepA A { get; }
        public IDepB B { get; }

        public LargeTestClass(IDepA a, IDepB b)
        {
            A = a;
            B = b;
        }
    }

}
