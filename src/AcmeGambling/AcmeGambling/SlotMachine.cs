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
                throw new ArgumentException("Balance cannot be 0");
            }

            Balance = amount;
        }

        /// <inheritdoc cref="ISlotMachine.Spin"/>
        public decimal Spin(decimal steak)
        {
            return 0m;
        }
    }
}