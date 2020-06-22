namespace AcmeGambling.Symbols
{
    /// <summary>
    /// Class representing a reel symbol
    /// </summary>
    public abstract class Symbol
    {
        /// <summary>
        /// Placeholder holding the character of the symbol
        /// </summary>
        public abstract string Character { get; protected set; }

        /// <summary>
        /// The win coefficient
        /// </summary>
        public abstract decimal WinCoefficient { get; protected set; }

        /// <summary>
        /// Appearance percentage
        /// </summary>
        public abstract byte AppearanceProbability { get; protected set; }
    }
}
