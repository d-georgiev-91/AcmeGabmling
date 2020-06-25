using AcmeGambling.Settings;
using Microsoft.Extensions.Options;

namespace AcmeGambling.Symbols
{
    /// <inheritdoc cref="Symbol"/>
    public class Banana : Symbol
    {
        public Banana(SymbolSetting symbolsSettings) :
            base(symbolsSettings)
        {
        }
    }
}