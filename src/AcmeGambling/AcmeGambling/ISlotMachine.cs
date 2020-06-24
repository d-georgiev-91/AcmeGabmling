namespace AcmeGambling
{
    /// <summary>
    /// Slot machine
    /// </summary>
    public interface ISlotMachine
    {
        /// <summary>
        /// Deposited amount into play
        /// </summary>
        decimal Balance { get; }

        /// <summary>
        /// Deposit amount into balance
        /// </summary>
        void Deposit(decimal amount);

        /// <summary>
        /// Spins the sloth machine reels and updates balance base on if it's a win or lose
        /// </summary>
        /// <param name="steak">The steak</param>
        void Spin(decimal steak);
    }
}
