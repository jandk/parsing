using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Parsing
{
    [Serializable]
    public class TokenizerException : Exception
    {
        public Location Location { get; private set; }

        public TokenizerException(string message, Location location)
            : this(message, location, null)
        {
        }

        public TokenizerException(string message, Location location, Exception inner)
            : base(message, inner)
        {
            Location = location;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(String.Format("Tokenizer Error: {0}", Message));
            sb.AppendLine("\t" + Location);
            return sb.ToString();
        }
    }
}
