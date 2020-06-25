using AcmeGambling.Settings;

namespace AcmeGambling.Symbols
{
    /// <summary>
    /// Class representing a reel symbol
    /// </summary>
    public class Symbol
    {
        protected Symbol(SymbolSetting symbolSetting)
        {
            Character = symbolSetting.Character;
            WinCoefficient = symbolSetting.WinCoefficient;
            AppearanceProbability = symbolSetting.AppearanceProbability;
        }

        /// <summary>
        /// Placeholder holding the character of the symbol
        /// </summary>
        public string Character { get; }

        /// <summary>
        /// The win coefficient
        /// </summary>
        public decimal WinCoefficient { get; }

        /// <summary>
        /// Appearance percentage
        /// </summary>
        public byte AppearanceProbability { get; }
    }
}
