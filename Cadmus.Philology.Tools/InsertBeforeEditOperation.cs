using System.Text;
using System.Text.RegularExpressions;

namespace Cadmus.Philology.Tools;

public class InsertBeforeEditOperation : EditOperation
{
    public override OperationType Type => OperationType.InsertBefore;
    public string ReplacementText { get; set; } = string.Empty;

    public override string Execute(string input)
    {
        if (Position == 0)
        {
            return ReplacementText + input;
        }
        ValidatePosition(input, Position);
        return input.Insert(Position - 1, ReplacementText);
    }

    public override void Parse(string dslText)
    {
        // Pattern: @N+="B"
        string pattern = @"@(\d+)\s*\+=\s*""([^""]*)""";
        Match match = Regex.Match(dslText, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ParseException("Invalid insert-before operation format. Expected: @position+=\"text\"", dslText);
        }

        if (!int.TryParse(match.Groups[1].Value, out int position) || position < 0)
        {
            throw new ParseException("Position must be a non-negative integer", match.Groups[1].Value);
        }
        Position = position;

        ReplacementText = match.Groups[2].Value;
        ParseNoteAndTags(dslText);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"@{Position}+=\"{ReplacementText}\"");

        if (!string.IsNullOrEmpty(Note))
            sb.Append($" ({Note})");

        if (Tags.Any())
            sb.Append($" [{string.Join(" ", Tags)}]");

        return sb.ToString();
    }
}
