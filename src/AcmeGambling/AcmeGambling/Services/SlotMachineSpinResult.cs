using AcmeGambling.Symbols;

namespace AcmeGambling.Services
{
    /// <summary>
    /// Wrapper class providing data after slot machine is spun
    /// </summary>
    public class SlotMachineSpinResult
    {
        /// <summary>
        /// Gets/sets the won amount
        /// </summary>
        public decimal AmountWon { get; set; }

        /// <summary>
        /// The generated symbols for spin of machine reels
        /// </summary>
        public Symbol[,] Reels { get; set; }
    }
}