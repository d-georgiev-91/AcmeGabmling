using NSubstitute;
using NUnit.Framework;

namespace AcmeGambling.Tests
{
    [TestFixture]
    public class SlotMachineTests
    {
        private ISlotMachine _slotMachine;
        private IRandomSymbolGenerator _randomSymbolGenerator;

        [SetUp]
        public void Setup()
        {
            _randomSymbolGenerator = Substitute.For<IRandomSymbolGenerator>();
            _slotMachine = new SlotMachine(_randomSymbolGenerator);
        }

        [Test]
        public void WhenDepositWith0IsCalledThenArgumentExceptionShouldBeThrownAndBalanceShouldRemainTheSame()
        {
            var balance = _slotMachine.Balance;
            Assert.That(() => { _slotMachine.Deposit(0); }, Throws.ArgumentException);
            Assert.That(_slotMachine.Balance, Is.EqualTo(balance));
        }

        [Test]
        public void WhenDepositWithNegativeValueIsCalledThenArgumentExceptionShouldBeThrownAndBalanceShouldRemainTheSame()
        {
            var balance = _slotMachine.Balance;
            Assert.That(() => { _slotMachine.Deposit(-2); }, Throws.ArgumentException);
            Assert.That(_slotMachine.Balance, Is.EqualTo(balance));
        }

        [Test]
        public void WhenDepositWith5ValueIsCalledThenBalanceShouldIncreaseBy5()
        {
            var balanceBeforeDeposit = _slotMachine.Balance;
            var depositAmount = 5;
            var expectedBalanceAfterDeposit = balanceBeforeDeposit + depositAmount;
            _slotMachine.Deposit(depositAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalanceAfterDeposit));
        }
    }
}