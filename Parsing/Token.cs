namespace Parsing
{
    public abstract class Token
        : IToken
    {
        private readonly Location _location;

        protected Token(Location location)
        {
            _location = location;
        }

        public Location Location
        {
            get { return _location; }
        }
    }
}
