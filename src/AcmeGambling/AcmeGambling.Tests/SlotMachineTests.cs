using NUnit.Framework;

namespace AcmeGambling.Tests
{
    [TestFixture]
    public class SlotMachineTests
    {
        private ISlotMachine _slotMachine;

        [SetUp]
        public void Setup()
        {
            _slotMachine = new SlotMachine();
        }
        
        [Test]
        public void SpinTest()
        {
            var reward  = _slotMachine.Spin(3);
            Assert.That(reward, Is.Zero);
        }
    }
}