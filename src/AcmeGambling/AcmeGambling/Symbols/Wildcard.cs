using AcmeGambling.Settings;
using Microsoft.Extensions.Options;

namespace AcmeGambling.Symbols
{
    /// <inheritdoc cref="Symbol"/>
    public class Wildcard : Symbol
    {
        public Wildcard(SymbolSetting symbolsSettings) :
            base(symbolsSettings)
        {
        }
    }
}
