using System.Collections.Generic;
using AcmeGambling.Symbols;

namespace AcmeGambling.Services
{
    public interface ISymbolsProvider
    {
        IReadOnlyCollection<Symbol> GetSymbols();
    }
}