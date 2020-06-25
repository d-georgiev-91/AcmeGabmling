using AcmeGambling.Symbols;

namespace AcmeGambling.Services
{
    /// <summary>
    /// Wrapper class providing data after slot machine is spun
    /// </summary>
    public class SlotMachineSpinResult
    {
        /// <summary>
        /// Get/set if spin result is win or lose
        /// </summary>
        public bool IsWin { get; set; }

        /// <summary>
        /// The generated symbols for spin of machine reels
        /// </summary>
        public Symbol[,] Reels { get; set; }
    }
}