using System.Collections.Immutable;

namespace Dependencies.Check.Models
{
    public record CircularReferenceError
    {
        public CircularReferenceError(IImmutableList<string> references)
        {
            References = references;
        }

        public IImmutableList<string> References { get; }
    }
}
