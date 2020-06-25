using AcmeGambling.Settings;
using Microsoft.Extensions.Options;

namespace AcmeGambling.Symbols
{
    /// <inheritdoc cref="Symbol"/>
    public class Pineapple : Symbol
    {
        public Pineapple(SymbolSetting symbolsSettings) :
            base(symbolsSettings)
        {
        }
    }
}
