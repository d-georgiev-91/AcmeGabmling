using System;
using System.Collections.Generic;
using AcmeGambling.Settings;
using AcmeGambling.Symbols;
using Microsoft.Extensions.Options;

namespace AcmeGambling.Services
{
    public class SymbolsProvider : ISymbolsProvider
    {
        private readonly IOptions<SymbolsSettings> _symbolsSettings;

        public SymbolsProvider(IOptions<SymbolsSettings> symbolsSettings)
        {
            _symbolsSettings = symbolsSettings;
        }

        /// <inheritdoc cref="ISymbolsProvider.GetSymbols" />
        public IReadOnlyCollection<Symbol> GetSymbols()
        {
            var symbols = new List<Symbol>
            {
                new Apple(_symbolsSettings.Value.Apple),
                new Banana(_symbolsSettings.Value.Banana),
                new Pineapple(_symbolsSettings.Value.Pineapple),
                new Wildcard(_symbolsSettings.Value.WildCard)
            };

            return symbols;
        }
    }
}