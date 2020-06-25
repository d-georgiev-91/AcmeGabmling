using System.Collections.Generic;
using System.IO;
using AcmeGambling.Services;
using AcmeGambling.Settings;
using AcmeGambling.Symbols;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace AcmeGambling.Tests
{
    [TestFixture]
    public class SlotMachineTests
    {
        private ISlotMachine _slotMachine;
        private IRandomSymbolGenerator _randomSymbolGenerator;
        
        private Apple _apple;
        private IOptions<SymbolsSettings> _symbolSettingsOptions;
        private Pineapple _pineapple;
        private Banana _banana;
        private Wildcard _wildCard;

        [OneTimeSetUp]
        public void OneTineSetup()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json")
                .Build();

            var symbolsSettings = new SymbolsSettings();

            configuration.GetSection("Symbols").Bind(symbolsSettings);
            _symbolSettingsOptions = Options.Create(symbolsSettings);
        }

        [SetUp]
        public void Setup()
        {
            _apple = new Apple(_symbolSettingsOptions.Value.Apple);
            _pineapple = new Pineapple(_symbolSettingsOptions.Value.Pineapple);
            _banana = new Banana(_symbolSettingsOptions.Value.Banana);
            _wildCard = new Wildcard(_symbolSettingsOptions.Value.WildCard);

            _randomSymbolGenerator = Substitute.For<IRandomSymbolGenerator>();
            var slotMachineOptions = Substitute.For<IOptions<SlotMachineSettings>>();
            slotMachineOptions.Value.Returns(new SlotMachineSettings()
            {
                ReelSymbolsCount = 4,
                ReelsCount = 3,
            });

            var symbolsProvider = Substitute.For<ISymbolsProvider>();

            _slotMachine = new SlotMachine(_randomSymbolGenerator, 
                slotMachineOptions, 
                symbolsProvider);
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
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + 3 * _apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    _apple, _apple, _apple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + 2 * _apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    _apple, _wildCard, _apple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + _apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    _apple, _wildCard, _wildCard,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount + _apple.WinCoefficient * steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    _wildCard, _wildCard, _apple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount - steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    _banana, _wildCard, _apple,
                    _apple, _pineapple, _banana,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount - steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    _wildCard, _wildCard, _wildCard,
                    _apple, _pineapple, _banana,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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
            const decimal depositAmount = 100;
            const decimal steakAmount = 10;
            var expectedBalance = depositAmount - steakAmount;

            var nonWiningCombination = new Queue<Symbol>
            (
                new Symbol[]
                {
                    _banana, _apple, _wildCard,
                    _apple, _pineapple, _banana,
                    _apple, _banana, _pineapple,
                    _apple, _banana, _pineapple
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