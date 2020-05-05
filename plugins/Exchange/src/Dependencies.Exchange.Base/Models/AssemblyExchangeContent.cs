using System;
using System.Collections.Generic;
using System.Text;

namespace Dependencies.Exchange.Base.Models
{
    public class AssemblyExchangeContent
    {
        public AssemblyExchange Assembly { get; set; }
        public IList<AssemblyExchange> Dependencies { get; set; }

        //TODO extensions method
        public void Deconstruct(out AssemblyExchange assembly, out IList<AssemblyExchange> dependencies)
        {
            assembly = Assembly;
            dependencies = Dependencies;
        }
    }
}
