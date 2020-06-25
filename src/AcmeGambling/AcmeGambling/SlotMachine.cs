using System;
using System.Collections.Generic;
using AcmeGambling.Symbols;

namespace AcmeGambling
{
    /// <inheritdoc cref="ISlotMachine"/>
    public class SlotMachine : ISlotMachine
    {
        private const int InitialBalance = 0;

        // TODO: Inject and configure
        private const int ReelsCount = 3;
        private const int ReelSymbolsCount = 4;

        private static readonly IReadOnlyCollection<Symbol> PossibleSymbols = new List<Symbol>
        {
            new Apple(),
            new Banana(),
            new Pineapple(),
            new Wildcard()
        };

        private readonly IRandomSymbolGenerator _randomSymbolGenerator;

        public SlotMachine(IRandomSymbolGenerator randomSymbolGenerator)
        {
            _randomSymbolGenerator = randomSymbolGenerator;
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
        public void Spin(decimal steak)
        {
            if (steak > Balance)
            {
                throw new ArgumentException($"{nameof(steak)} should be less or equal to {nameof(Balance)}");
            }

            var reels = GenerateReels();

            var totalWinCoefficient =  CalculateTotalWinCoefficient(reels);

            if (totalWinCoefficient > 0)
            {
                Balance += steak * totalWinCoefficient;
            }
            else
            {
                Balance -= steak;
            }
        }

        private Symbol[,] GenerateReels()
        {
            var machine = new Symbol[ReelSymbolsCount, ReelsCount];

            for (int symbolIndex = 0; symbolIndex < ReelSymbolsCount; symbolIndex++)
            {

                for (int reelIndex = 0; reelIndex < ReelsCount; reelIndex++)
                {
                    var symbol = _randomSymbolGenerator.Generate(PossibleSymbols);
                    machine[symbolIndex, reelIndex] = symbol;
                }
            }

            return machine;
        }

        private decimal CalculateTotalWinCoefficient(Symbol[,] reels)
        {
            var totalWinCoefficient = 0m;

            for (int symbolIndex = 0; symbolIndex < ReelSymbolsCount; symbolIndex++)
            {
                var symbol = reels[symbolIndex, 0];
                var rowWinCoefficient = symbol.WinCoefficient;

                for (int reelIndex = 1; reelIndex < ReelsCount; reelIndex++)
                {
                    var currentSymbol = reels[symbolIndex, reelIndex];

                    if (symbol.GetType() == typeof(Wildcard))
                    {
                        currentSymbol = symbol;
                    }

                    if (currentSymbol.GetType() == typeof(Wildcard))
                    {
                        continue;
                    }

                    if (currentSymbol.GetType() != symbol.GetType())
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