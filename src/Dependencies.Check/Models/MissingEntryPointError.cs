using System.Collections.Immutable;

namespace Dependencies.Check.Models
{
    public record MissingEntryPointError
    {
        public MissingEntryPointError(string name, string target, IImmutableList<string> missingMethods)
        {
            Name = name;
            Target = target;
            MissingMethods = missingMethods;
        }

        public string Name { get; }
        
        public string Target { get; }

        public IImmutableList<string> MissingMethods { get; }
    }
}
