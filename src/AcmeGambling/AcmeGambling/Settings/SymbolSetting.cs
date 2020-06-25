namespace AcmeGambling.Settings
{
    public class SymbolSetting
    {
        /// <summary>
        /// Placeholder holding the character of the symbol
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// The win coefficient
        /// </summary>
        public decimal WinCoefficient { get; set; }

        /// <summary>
        /// Appearance percentage
        /// </summary>
        public byte AppearanceProbability { get; set; }
    }
}