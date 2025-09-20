namespace Cadmus.Philology.Tools;

public class ParseException : Exception
{
    public string InputSubstring { get; }
    public int Position { get; }

    public ParseException(string message, string inputSubstring, int position = -1)
        : base(message)
    {
        InputSubstring = inputSubstring;
        Position = position;
    }
}
