using System.Text.RegularExpressions;

namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// Represents a JSON string.
/// </summary>
public struct StringJ : IJSON
{
    string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringJ"/> struct with an empty string.
    /// </summary>
    public StringJ()
    {
        _value = "";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringJ"/> struct by parsing a JSON string from the specified position.
    /// </summary>
    /// <param name="s">The JSON string to parse.</param>
    /// <param name="index">The position to start parsing from.</param>
    /// <exception cref="FormatException">Thrown when the string is not in a valid JSON format.</exception>
    public StringJ(string s, int index = 0)
    {
        Regex expression = new Regex(@"^\x22([\x20-\x21\x23-\x5B\x5D-\uFFFF]|\p{L}|\x5C[\x22\x5C\x2F\x62\x66\x6E\x72\x74]|\x5C\x75[0-9a-fA-F]{4})*\x22");
        Match match = expression.Match(s[index..]);
        if (!match.Success)
        {
            throw new FormatException("Неверный формат json");
        }
        _value = match.Value[1..^1];
    }

    /// <summary>
    /// Gets the string value.
    /// </summary>
    string Value => _value;

    /// <summary>
    /// Implicit conversion from <see cref="StringJ"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="s">The <see cref="StringJ"/> instance.</param>
    public static implicit operator string(StringJ s)
    {
        return s.Value;
    }

    /// <summary>
    /// Implicit conversion from <see cref="string"/> to <see cref="StringJ"/>.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    public static implicit operator StringJ(string s)
    {
        return new StringJ { _value = s };
    }

    /// <summary>
    /// Converts the <see cref="StringJ"/> to a string.
    /// </summary>
    /// <returns>The JSON string representation of the <see cref="StringJ"/>.</returns>
    public override string ToString()
    {
        return '"' + _value + '"';
    }

    /// <summary>
    /// Converts the <see cref="StringJ"/> to a human-readable string.
    /// </summary>
    /// <returns>The human-readable string representation of the <see cref="StringJ"/>.</returns>
    public string HumanReadable()
    {
        return ToString();
    }
}
