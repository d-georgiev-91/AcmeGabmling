using System.Collections.Generic;
using AcmeGambling.Symbols;

namespace AcmeGambling.Services
{
    /// <summary>
    /// Random symbols generator
    /// </summary>
    public interface IRandomSymbolGenerator
    {
        /// <summary>
        /// Generates a random symbol from allowed symbols
        /// </summary>
        /// <param name="symbols">The allowed symbols</param>
        /// <returns>Random symbol</returns>
        Symbol Generate(IReadOnlyCollection<Symbol> symbols);
    }
}