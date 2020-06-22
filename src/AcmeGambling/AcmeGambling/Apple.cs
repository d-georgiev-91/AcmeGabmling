namespace AcmeGambling
{

    /// <inheritdoc cref="Symbol"/>
    public class Apple : Symbol
    {
        public Apple()
        {
            Character = "A";
            WinCoefficient = 0.4M;
            AppearanceProbability = 45;
        }

        /// <inheritdoc cref="Symbol.Character"/>
        public sealed override string Character { get; protected set; }

        /// <inheritdoc cref="Symbol.WinCoefficient"/>
        public sealed override decimal WinCoefficient { get; protected set; }

        /// <inheritdoc cref="Symbol.AppearanceProbability"/>
        public sealed override byte AppearanceProbability { get; protected set; }
    }
}