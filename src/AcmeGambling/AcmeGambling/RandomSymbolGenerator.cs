using System;
using System.Collections.Generic;
using System.Linq;
using AcmeGambling.Symbols;

namespace AcmeGambling
{
    /// <summary>
    /// Trivial random symbols generator
    /// </summary>
    public class RandomSymbolGenerator : IRandomSymbolGenerator
    {
        private readonly Random _random;

        public RandomSymbolGenerator()
        {
            _random = new Random();
        }

        /// <inheritdoc cref="IRandomSymbolGenerator.Generate"/>
        public Symbol Generate(IReadOnlyCollection<Symbol> symbols)
        {
            var totalAppearanceProbability = symbols.Sum(x => x.AppearanceProbability);
            Symbol winningSector = null;

            var rgn = _random.Next(0, totalAppearanceProbability);
            var sum = 0;

            foreach (var symbol in symbols)
            {
                sum += symbol.AppearanceProbability;
                if (rgn < sum)
                {
                    winningSector = symbol;
                    break;
                }
            }

            return winningSector;
        }
    }
}
