using System.Text;
using System.Text.RegularExpressions;

namespace Cadmus.Philology.Tools;

public class SwapEditOperation : EditOperation
{
    public override OperationType Type => OperationType.Swap;
    public int SecondPosition { get; set; }
    public int SecondLength { get; set; } = 1;
    public string? SecondInputText { get; set; }

    public override string Execute(string input)
    {
        ValidatePosition(input, Position, Length);
        ValidatePosition(input, SecondPosition, SecondLength);

        if (Position == SecondPosition || (Position < SecondPosition && Position + Length > SecondPosition) ||
            (SecondPosition < Position && SecondPosition + SecondLength > Position))
        {
            throw new ArgumentException("Swap positions cannot overlap");
        }

        string firstText = input.Substring(Position - 1, Length);
        string secondText = input.Substring(SecondPosition - 1, SecondLength);

        StringBuilder result = new(input);

        // Replace in order of highest position first to avoid index shifting
        if (Position > SecondPosition)
        {
            result.Remove(Position - 1, Length);
            result.Insert(Position - 1, secondText);
            result.Remove(SecondPosition - 1, SecondLength);
            result.Insert(SecondPosition - 1, firstText);
        }
        else
        {
            result.Remove(SecondPosition - 1, SecondLength);
            result.Insert(SecondPosition - 1, firstText);
            result.Remove(Position - 1, Length);
            result.Insert(Position - 1, secondText);
        }

        return result.ToString();
    }

    public override void Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        // Pattern: "A"@NxN<>"B"@MxM or @NxN<>@MxM
        string pattern = @"(?:""([^""]*)"")?\s*@(\d+)(?:[x×](\d+))?\s*<>\s*(?:""([^""]*)"")?\s*@(\d+)(?:[x×](\d+))?";
        Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ParseException("Invalid swap operation format. Expected: \"text1\"@position1<>\"text2\"@position2", text);
        }

        InputText = match.Groups[1].Success ? match.Groups[1].Value : null;

        if (!int.TryParse(match.Groups[2].Value, out int position) || position < 1)
        {
            throw new ParseException("First position must be a positive integer", match.Groups[2].Value);
        }
        Position = position;

        Length = 1;
        if (match.Groups[3].Success)
        {
            if (!int.TryParse(match.Groups[3].Value, out int length) || length < 1)
            {
                throw new ParseException("First length must be a positive integer", match.Groups[3].Value);
            }
            Length = length;
        }

        SecondInputText = match.Groups[4].Success ? match.Groups[4].Value : null;

        if (!int.TryParse(match.Groups[5].Value, out int secondPosition) || secondPosition < 1)
        {
            throw new ParseException("Second position must be a positive integer", match.Groups[5].Value);
        }
        SecondPosition = secondPosition;

        SecondLength = 1;
        if (match.Groups[6].Success)
        {
            if (!int.TryParse(match.Groups[6].Value, out int secondLength) || secondLength < 1)
            {
                throw new ParseException("Second length must be a positive integer", match.Groups[6].Value);
            }
            SecondLength = secondLength;
        }

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
        sb.Append("<>");

        if (!string.IsNullOrEmpty(SecondInputText))
            sb.Append($"\"{SecondInputText}\"");

        sb.Append($"@{SecondPosition}");
        if (SecondLength > 1) sb.Append($"x{SecondLength}");

        if (!string.IsNullOrEmpty(Note)) sb.Append($" ({Note})");

        if (Tags.Count > 0) sb.Append($" [{string.Join(" ", Tags)}]");

        return sb.ToString();
    }
}
