using System.Text;
using System.Text.RegularExpressions;

namespace Cadmus.Philology.Tools;

public class ReplaceEditOperation : EditOperation
{
    public override OperationType Type => OperationType.Replace;
    public string ReplacementText { get; set; } = string.Empty;

    public override string Execute(string input)
    {
        ValidatePosition(input, Position, Length);
        StringBuilder result = new StringBuilder(input);
        result.Remove(Position - 1, Length);
        result.Insert(Position - 1, ReplacementText);
        return result.ToString();
    }

    public override void Parse(string dslText)
    {
        // Pattern: "A"@NxN="B" or @NxN="B"
        string pattern = @"(?:""([^""]*)"")?\s*@(\d+)(?:[x×](\d+))?\s*=\s*""([^""]*)""";
        Match match = Regex.Match(dslText, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ParseException("Invalid replace operation format. Expected: \"oldtext\"@position=\"newtext\" or @position=\"newtext\"", dslText);
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
        ParseNoteAndTags(dslText);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (!string.IsNullOrEmpty(InputText))
            sb.Append($"\"{InputText}\"");

        sb.Append($"@{Position}");
        if (Length > 1)
            sb.Append($"x{Length}");
        sb.Append($"=\"{ReplacementText}\"");

        if (!string.IsNullOrEmpty(Note))
            sb.Append($" ({Note})");

        if (Tags.Any())
            sb.Append($" [{string.Join(" ", Tags)}]");

        return sb.ToString();
    }
}
