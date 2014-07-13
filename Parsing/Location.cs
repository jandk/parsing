namespace Parsing
{
    /// <summary>
    ///  Holds a location in a text file.
    /// </summary>
    public class Location
    {
        public string File { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public override string ToString()
        {
            return string.Format(@"file: ""{0}"" at line {1}, column {2}", File, Line, Column);
        }
    }
}
