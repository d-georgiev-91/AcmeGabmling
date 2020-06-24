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
        /// Spins the sloth machine reels
        /// </summary>
        /// <param name="steak">The steak</param>
        /// <returns>The won amount of won reward</returns>
        decimal Spin(decimal steak);
    }
}
