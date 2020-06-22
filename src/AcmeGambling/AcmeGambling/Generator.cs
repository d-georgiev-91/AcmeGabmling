using System;
using System.Collections.Generic;
using System.Linq;
using AcmeGambling.Symbols;

namespace AcmeGambling
{
    public class Generator
    {
        private readonly Random _random = new Random();

        public Symbol GetRandomSymbol(IReadOnlyCollection<Symbol> symbols)
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
