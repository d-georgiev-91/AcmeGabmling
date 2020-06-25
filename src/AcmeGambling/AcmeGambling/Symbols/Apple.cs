using AcmeGambling.Settings;
using Microsoft.Extensions.Options;

namespace AcmeGambling.Symbols
{

    /// <inheritdoc cref="Symbol"/>
    public class Apple : Symbol
    {
        public Apple(SymbolSetting symbolsSetting) :
            base(symbolsSetting)
        {
        }
    }
}