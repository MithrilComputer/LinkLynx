using LinkLynx.Core.CrestronPOCOs;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Helpers;
using LinkLynx.Core.Signals;
using LinkLynx.Implementations.Collections.Registries;
using LinkLynx.Tests.GeneralFixtures;
using LinkLynx.Tests.GeneralMocks;
using NUnit.Framework;

namespace LinkLynx.Tests.UnitTests.SignalHelper.Scenarios
{
    [TestFixture]
    public class GeneralTests
    {
        [Test, Category("Unit Tests")]
        public void BasicUseTest()
        {
            ILogger logger = new TestLogger();

            IEnumSignalTypeRegistry typeRegistry = new EnumSignalTypeRegistry(logger);

            ISignalHelper signalHelper = new Implementations.Utility.Helpers.SignalHelper(typeRegistry, logger);

            typeRegistry.Register(typeof(TestButtons), SigType.Bool);

            PanelDevice panel = new PanelDevice(0x03, true, "TestPanel", logger);

            signalHelper.SetLogicJoin(panel, TestButtons.Button1, true);

            Assert.That(logger.Messages.Last(), Does.Contain("[PanelDevice] Panel's BasicTriList is null on Panel Device"));
        }

        [Test, Category("Unit Tests")]
        public void NullPanelUseTest()
        {
            ILogger logger = new TestLogger();

            IEnumSignalTypeRegistry typeRegistry = new EnumSignalTypeRegistry(logger);

            ISignalHelper signalHelper = new Implementations.Utility.Helpers.SignalHelper(typeRegistry, logger);

            typeRegistry.Register(typeof(TestButtons), SigType.Bool);

            PanelDevice panel = null;

            Assert.Throws<ArgumentNullException>(() => signalHelper.SetLogicJoin(panel, TestButtons.Button1, true));
        }

        [Test, Category("Unit Tests")]
        public void IncorrectTypeUseTest()
        {
            ILogger logger = new TestLogger();

            IEnumSignalTypeRegistry typeRegistry = new EnumSignalTypeRegistry(logger);

            ISignalHelper signalHelper = new Implementations.Utility.Helpers.SignalHelper(typeRegistry, logger);

            typeRegistry.Register(typeof(TestButtons), SigType.Bool);

            PanelDevice panel = new PanelDevice(0x03, true, "TestPanel", logger);

            Assert.Throws<ArgumentException>(() => signalHelper.SetLogicJoin(panel, TestButtons.Button1, "Fail Me"));
        }

        [Test, Category("Unit Tests")]
        public void UnregisteredEnumTest()
        {
            ILogger logger = new TestLogger();

            IEnumSignalTypeRegistry typeRegistry = new EnumSignalTypeRegistry(logger);

            ISignalHelper signalHelper = new Implementations.Utility.Helpers.SignalHelper(typeRegistry, logger);

            PanelDevice panel = new PanelDevice(0x03, true, "TestPanel", logger);

            Assert.Throws<KeyNotFoundException>(() => signalHelper.SetLogicJoin(panel, TestButtons.Button1, true));
        }

        [Test, Category("Unit Tests")]
        public void NullEnumTest()
        {
            ILogger logger = new TestLogger();

            IEnumSignalTypeRegistry typeRegistry = new EnumSignalTypeRegistry(logger);

            ISignalHelper signalHelper = new Implementations.Utility.Helpers.SignalHelper(typeRegistry, logger);

            PanelDevice panel = new PanelDevice(0x03, true, "TestPanel", logger);

            Assert.Throws<ArgumentNullException>(() => signalHelper.SetLogicJoin(panel, null, true));
        }
    }
}
