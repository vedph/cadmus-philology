using System.Text;
using System.Text.RegularExpressions;

namespace Cadmus.Philology.Tools;

public class MoveBeforeEditOperation : EditOperation
{
    public override OperationType Type => OperationType.MoveBefore;
    public int TargetPosition { get; set; }

    public override string Execute(string input)
    {
        ValidatePosition(input, Position, Length);
        ValidatePosition(input, TargetPosition);

        string textToMove = input.Substring(Position - 1, Length);
        StringBuilder result = new StringBuilder(input);

        // Remove the text first
        result.Remove(Position - 1, Length);

        // Adjust target position if it's after the removed text
        int adjustedTargetPosition = TargetPosition;
        if (TargetPosition > Position)
        {
            adjustedTargetPosition -= Length;
        }

        // Insert at target position
        result.Insert(adjustedTargetPosition - 1, textToMove);

        return result.ToString();
    }

    public override void Parse(string dslText)
    {
        // Pattern: "A"@N>@M or @N>@M
        string pattern = @"(?:""([^""]*)"")?\s*@(\d+)(?:[x×](\d+))?\s*>\s*@(\d+)";
        Match match = Regex.Match(dslText, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ParseException("Invalid move-before operation format. Expected: \"text\"@position>@targetposition or @position>@targetposition", dslText);
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

        if (!int.TryParse(match.Groups[4].Value, out int targetPosition) || targetPosition < 1)
        {
            throw new ParseException("Target position must be a positive integer", match.Groups[4].Value);
        }
        TargetPosition = targetPosition;

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
        sb.Append($">@{TargetPosition}");

        if (!string.IsNullOrEmpty(Note))
            sb.Append($" ({Note})");

        if (Tags.Any())
            sb.Append($" [{string.Join(" ", Tags)}]");

        return sb.ToString();
    }
}
