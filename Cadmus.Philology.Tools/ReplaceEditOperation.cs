using System.Text;
using System.Text.RegularExpressions;

namespace Cadmus.Philology.Tools;

public class ReplaceEditOperation : EditOperation
{
    public override OperationType Type => OperationType.Replace;
    public string ReplacementText { get; set; } = string.Empty;

    public override string Execute(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        ValidatePosition(input, Position, Length);
        StringBuilder result = new(input);
        result.Remove(Position - 1, Length);
        result.Insert(Position - 1, ReplacementText);
        return result.ToString();
    }

    public override void Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        // pattern: "A"@NxN="B" or @NxN="B"
        string pattern = @"(?:""([^""]*)"")?\s*@(\d+)(?:[x×](\d+))?\s*=\s*""([^""]*)""";
        Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ParseException("Invalid replace operation format. " +
                "Expected: \"oldtext\"@position=\"newtext\" " +
                "or @position=\"newtext\"", text);
        }

        InputText = match.Groups[1].Success ? match.Groups[1].Value : null;

        if (!int.TryParse(match.Groups[2].Value, out int position) || position < 1)
        {
            throw new ParseException("Position must be a positive integer", match.Groups[2].Value);
        }
        Position = position;

        Length = 1;
        if (match.Groups[3].Success)
        {
            if (!int.TryParse(match.Groups[3].Value, out int length) || length < 1)
            {
                throw new ParseException("Length must be a positive integer", match.Groups[3].Value);
            }
            Length = length;
        }

        ReplacementText = match.Groups[4].Value;
        ParseNoteAndTags(text);
    }

    /// <summary>
    /// Returns a parsable string representation of the object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        StringBuilder sb = new();

        if (!string.IsNullOrEmpty(InputText)) sb.Append($"\"{InputText}\"");

        sb.Append($"@{Position}");
        if (Length > 1) sb.Append($"x{Length}");
        sb.Append($"=\"{ReplacementText}\"");

        if (!string.IsNullOrEmpty(Note)) sb.Append($" ({Note})");

        if (Tags.Count > 0) sb.Append($" [{string.Join(" ", Tags)}]");

        return sb.ToString();
    }
}
