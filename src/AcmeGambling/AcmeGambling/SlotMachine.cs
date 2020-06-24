using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new ArgumentException("Balance cannot be 0");
            }

            Balance = amount;
        }

        /// <inheritdoc cref="ISlotMachine.Spin"/>
        public void Spin(decimal steak)
        {
            var machine = new Symbol[ReelSymbolsCount, ReelsCount];
            var winCoefficient = 0m;

            for (int symbolIndex = 0; symbolIndex < ReelSymbolsCount; symbolIndex++)
            {
                var matches = new Dictionary<Symbol, int>();

                for (int reelIndex = 0; reelIndex < ReelsCount; reelIndex++)
                {
                    var symbol = _randomSymbolGenerator.Generate(PossibleSymbols);

                    Console.Write(symbol.Character);

                    machine[symbolIndex, reelIndex] = symbol;

                    if (!matches.ContainsKey(symbol))
                    {
                        matches.Add(symbol, 0);
                    }

                    matches[symbol]++;
                }

                var mostFrequentSymbol = matches
                    .OrderByDescending(m => m.Value)
                    .First();

                if (mostFrequentSymbol.Value == ReelsCount ||
                    mostFrequentSymbol.Value == ReelsCount - 1 && matches.Count(s => s.Key is Wildcard) == 1 ||
                    mostFrequentSymbol.Value == 1 && matches.Count(s => s.Key is Wildcard) == ReelsCount - 1
                )
                {
                    winCoefficient += mostFrequentSymbol.Key.WinCoefficient * mostFrequentSymbol.Value;
                }

                Console.WriteLine();
            }

            Console.WriteLine(new string('-', ReelsCount));

            Balance += winCoefficient * steak;
        }
    }
}