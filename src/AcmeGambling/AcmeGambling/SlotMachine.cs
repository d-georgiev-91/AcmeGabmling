using System;

namespace AcmeGambling
{
    /// <inheritdoc cref="ISlotMachine"/>
    public class SlotMachine : ISlotMachine
    {
        private const int InitialBalance = 0;

        public SlotMachine()
        {
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