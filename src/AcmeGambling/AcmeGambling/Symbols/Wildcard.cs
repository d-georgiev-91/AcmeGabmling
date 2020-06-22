namespace AcmeGambling.Symbols
{
    /// <inheritdoc cref="Symbol"/>
    public class Wildcard : Symbol
    {
        public Wildcard() :
            base("*", 0, 5)
        {
        }
    }
}
