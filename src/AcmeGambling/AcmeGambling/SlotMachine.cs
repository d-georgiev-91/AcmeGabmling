using System;
using System.Collections.Generic;
using AcmeGambling.Settings;
using AcmeGambling.Symbols;
using Microsoft.Extensions.Options;

namespace AcmeGambling
{
    /// <inheritdoc cref="ISlotMachine"/>
    public class SlotMachine : ISlotMachine
    {
        private const int InitialBalance = 0;

        private readonly IRandomSymbolGenerator _randomSymbolGenerator;
        private readonly int _reelsCount;
        private readonly int _reelSymbolsCount;
        private ISymbolsProvider _symbolsProvider;

        public SlotMachine(IRandomSymbolGenerator randomSymbolGenerator, 
            IOptions<SlotMachineSettings> slotMachineSettings, ISymbolsProvider symbolsProvider)
        {
            _randomSymbolGenerator = randomSymbolGenerator;
            _symbolsProvider = symbolsProvider;
            _reelsCount = slotMachineSettings.Value.ReelsCount;
            _reelSymbolsCount = slotMachineSettings.Value.ReelSymbolsCount;
            Balance = InitialBalance;
        }

        /// <inheritdoc cref="ISlotMachine.Balance"/>
        public decimal Balance { get; private set; }

        /// <inheritdoc cref="ISlotMachine.Balance"/>
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException($"{nameof(Balance)} cannot be negative number");
            }

            Balance = amount;
        }

        /// <inheritdoc cref="ISlotMachine.Spin"/>
        public SlotMachineSpinResult Spin(decimal steak)
        {
            if (steak > Balance)
            {
                throw new ArgumentException($"{nameof(steak)} should be less or equal to {nameof(Balance)}");
            }

            var result = new SlotMachineSpinResult
            {
                Reels = GenerateReels(),
                IsWin = false
            };

            var totalWinCoefficient = CalculateTotalWinCoefficient(result.Reels);

            if (totalWinCoefficient > 0)
            {
                Balance += steak * totalWinCoefficient;
                result.IsWin = true;
            }
            else
            {
                Balance -= steak;
            }

            return result;
        }

        private Symbol[,] GenerateReels()
        {
            var machine = new Symbol[_reelSymbolsCount, _reelsCount];

            for (int symbolIndex = 0; symbolIndex < _reelSymbolsCount; symbolIndex++)
            {

                for (int reelIndex = 0; reelIndex < _reelsCount; reelIndex++)
                {
                    var symbol = _randomSymbolGenerator.Generate(_symbolsProvider.GetSymbols());
                    machine[symbolIndex, reelIndex] = symbol;
                }
            }

            return machine;
        }

        private decimal CalculateTotalWinCoefficient(Symbol[,] reels)
        {
            var totalWinCoefficient = 0m;

            for (int symbolIndex = 0; symbolIndex < _reelSymbolsCount; symbolIndex++)
            {
                var pivotSymbol = reels[symbolIndex, 0];
                var rowWinCoefficient = pivotSymbol.WinCoefficient;

                for (int reelIndex = 1; reelIndex < _reelsCount; reelIndex++)
                {
                    var currentSymbol = reels[symbolIndex, reelIndex];

                    if (pivotSymbol.GetType() == typeof(Wildcard))
                    {
                        pivotSymbol = currentSymbol;
                    }

                    if (currentSymbol.GetType() == typeof(Wildcard))
                    {
                        continue;
                    }

                    if (currentSymbol.GetType() != pivotSymbol.GetType())
                    {
                        rowWinCoefficient = 0m;
                        break;
                    }

                    rowWinCoefficient += currentSymbol.WinCoefficient;
                }

                totalWinCoefficient += rowWinCoefficient;
            }

            return totalWinCoefficient;
        }
    }
}