using Crestron.SimplSharpPro.DeviceSupport; // only for the type in the Func signature
using LinkLynx.Core.Logic.Pages;            // only for the type in the Func signature
using LinkLynx.Core.Utility.Registries;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;

namespace LinkLynx.Tests
{
    [TestFixture]
    public class PageRegistryTests
    {
        // Simple no-op factory we can store/compare in tests.
        // We won't invoke it, so no Crestron runtime needed.
        private static readonly Func<BasicTriList, PageLogicBase> FactoryA =
            _ => null;

        private static readonly Func<BasicTriList, PageLogicBase> FactoryB =
            _ => null;

        [SetUp]
        public void SetUp()
        {
            // Ensure a clean slate for every test
            PageRegistry.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            PageRegistry.Clear();
        }

        [Test]
        public void RegisterPage_AddsEntry_WhenKeyIsNew()
        {
            PageRegistry.RegisterPage(1, FactoryA);

            var all = PageRegistry.GetAllRegistries();
            Assert.That(all.Count, Is.EqualTo(1));
            Assert.That(all.ContainsKey(1), Is.True);
            Assert.That(all[1], Is.SameAs(FactoryA));
        }

        [Test]
        public void RegisterPage_Throws_WhenKeyExists()
        {
            PageRegistry.RegisterPage(1, FactoryA);

            var ex = Assert.Throws<ArgumentException>(() =>
                PageRegistry.RegisterPage(1, FactoryB));

            StringAssert.Contains("already contains a key for PageID: 1", ex.Message);
        }

        [Test]
        public void GetPage_ReturnsFactory_WhenKeyExists()
        {
            PageRegistry.RegisterPage(2, FactoryB);

            var got = PageRegistry.GetPage(2);

            // exact same delegate instance
            Assert.That(got, Is.SameAs(FactoryB));
        }

        [Test]
        public void GetPage_ReturnsNull_WhenKeyMissing()
        {
            var got = PageRegistry.GetPage(999);
            Assert.That(got, Is.Null);
        }

        [Test]
        public void GetAllRegistries_ReflectsCurrentEntries()
        {
            PageRegistry.RegisterPage(10, FactoryA);
            PageRegistry.RegisterPage(11, FactoryB);

            var all = PageRegistry.GetAllRegistries();
            Assert.That(all.Count, Is.EqualTo(2));
            Assert.That(all.ContainsKey(10), Is.True);
            Assert.That(all.ContainsKey(11), Is.True);
        }

        [Test]
        public void Clear_RemovesAllEntries()
        {
            PageRegistry.RegisterPage(1, FactoryA);
            PageRegistry.RegisterPage(2, FactoryB);

            PageRegistry.Clear();

            var all = PageRegistry.GetAllRegistries();
            Assert.That(all.Count, Is.EqualTo(0));
        }
    }
}