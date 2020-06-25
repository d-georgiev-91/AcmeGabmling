using System.Collections.Generic;
using AcmeGambling.Symbols;

namespace AcmeGambling
{
    public interface ISymbolsProvider
    {
        IReadOnlyCollection<Symbol> GetSymbols();
    }
}