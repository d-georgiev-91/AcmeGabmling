using System.Collections.Generic;
using AcmeGambling.Symbols;
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
            Assert.That(() => _slotMachine.Deposit(0), Throws.ArgumentException);
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
            const decimal depositAmount = 5;
            var expectedBalanceAfterDeposit = balanceBeforeDeposit + depositAmount;
            _slotMachine.Deposit(depositAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalanceAfterDeposit));
        }

        [Test]
        public void WhenNonWinningCombinationIsGeneratedAndSteakIsEqualToBalanceThenBalanceShouldBe0()
        {
            const decimal depositAmount = 100;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    new Apple(), new Banana(), new Pineapple(),
                    new Apple(), new Banana(), new Pineapple(),
                    new Apple(), new Banana(), new Pineapple(),
                    new Apple(), new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(depositAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(0));
            Assert.That(spinResult.IsWin, Is.False);
        }

        [Test]
        public void WhenNonWinningCombinationIsGeneratedAndThenBalanceShouldBeReducedWithStake()
        {
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            const decimal expectedBalance = depositAmount - steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    new Apple(), new Banana(), new Pineapple(),
                    new Apple(), new Banana(), new Pineapple(),
                    new Apple(), new Banana(), new Pineapple(),
                    new Apple(), new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.False);
        }

        [Test]
        public void WhenThereIsOnlyOneLineWithThreeWinningSymbolsAndTheyAreApplesThenDepositShouldIncreaseByTheSumOfWinCoefficientOfAllApplesMultipliedBySteak()
        {
            var apple = new Apple();
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + 3 * apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    apple, apple, apple,
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.True);
        }

        [Test]
        public void WhenThereIsOnlyOneLineWithTwoApplesAndOneWildCardThenDepositShouldIncreaseByTheSumOfWinCoefficientOfTheTwoAppleMultipliedBySteak()
        {
            var apple = new Apple();
            var wildCard = new Wildcard();
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + 2 * apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    apple, wildCard, apple,
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.True);
        }

        [Test]
        public void WhenThereIsOnlyOneLineWithOneAppleAndTwoWildCardsThenDepositShouldIncreaseByTheSumOfWinCoefficientOfTheAppleMultipliedBySteak()
        {
            var apple = new Apple();
            var wildCard = new Wildcard();
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    apple, wildCard, wildCard,
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.True);
        }

        [Test]
        public void WhenThereIsOnlyOneLineWithTwoWildCardsAndOneAppleThenDepositShouldIncreaseByTheSumOfWinCoefficientOfTheAppleMultipliedBySteak()
        {
            var apple = new Apple();
            var wildCard = new Wildcard();
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    wildCard, wildCard, apple,
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.True);
        }

        [Test]
        public void WhenFirstSymbolInFirstRowIsBananaAndSecondIsWildCardAndThirdSymbolIsAppleAndThereAreNoOtherWinningRowsBalanceShouldBeReduced()
        {
            var apple = new Apple();
            var wildCard = new Wildcard();
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount - steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    new Banana(), wildCard, apple,
                    apple, new Pineapple(), new Banana(),
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.False);
        }

        [Test]
        public void WhenFirstRowIsAllWildCardsAndThereAreNoOtherWinningRowsBalanceShouldBeReduced()
        {
            var apple = new Apple();
            var wildCard = new Wildcard();
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount - steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    wildCard, wildCard, wildCard,
                    apple, new Pineapple(), new Banana(),
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.False);
        }

        [Test]
        public void WhenFirstSymbolInFirstRowIsBananaAndSecondIsAppleCardAndThirdSymbolIsWildCardAndThereAreNoOtherWinningRowsBalanceShouldBeReduced()
        {
            var apple = new Apple();
            var wildCard = new Wildcard();
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount - steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    new Banana(), apple, wildCard,
                    apple, new Pineapple(), new Banana(),
                    apple, new Banana(), new Pineapple(),
                    apple, new Banana(), new Pineapple()
                }
            );

            _randomSymbolGenerator.Generate(Arg.Any<IReadOnlyCollection<Symbol>>())
                .Returns(_ => nonWiningCombination.Dequeue());
            _slotMachine.Deposit(depositAmount);
            var spinResult = _slotMachine.Spin(steakAmount);
            Assert.That(_slotMachine.Balance, Is.EqualTo(expectedBalance));
            Assert.That(spinResult.IsWin, Is.False);
        }

        [Test]
        public void WhenSteakIsMoreThanArgumentExceptionShouldThrown()
        {
            Assert.That(() => _slotMachine.Spin(100), Throws.ArgumentException);
        }
    }
}