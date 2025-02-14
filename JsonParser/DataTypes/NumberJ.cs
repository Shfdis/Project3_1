using System.Text.RegularExpressions;

namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// A class for parsing and representing json numbers
/// </summary>
public class NumberJ : IJSON
{
    private string _number;

    /// <summary>
    /// Creates a new instance of NumberJ with an empty string as the number
    /// </summary>
    public NumberJ()
    {
        _number = "";
    } 
    /// <summary>
    /// Gets the number as a double
    /// </summary>
    public double Number => double.Parse(_number);
    /// <summary>
    /// Implicitly converts NumberJ to double
    /// </summary>
    /// <param name="number">The NumberJ to convert</param>
    /// <returns>The double value of the NumberJ</returns>
    public static implicit operator double(NumberJ number)
    {
        return number.Number;
    }

    /// <summary>
    /// Implicitly converts double to NumberJ
    /// </summary>
    /// <param name="number">The double to convert</param>
    /// <returns>The NumberJ value of the double</returns>
    public static implicit operator NumberJ(double number)
    {
        return new NumberJ { _number = number.ToString() };
    }

    /// <summary>
    /// Parses the json and creates a new instance of NumberJ
    /// </summary>
    /// <param name="s">The json to parse</param>
    /// <param name="index">The position to start parsing from</param>
    public NumberJ(string s, int index)
    {
        Regex expression = new Regex(@"^\-?(0|[1-9][0-9]*)(\.[0-9]+)?([eE][-+]?[0-9]+)?");
        Match match = expression.Match(s[index..]);
        if (!match.Success)
        {
            throw new FormatException("Неверный формат");
        }
        _number = match.Value;
    }

    /// <summary>
    /// Converts the NumberJ to a string
    /// </summary>
    /// <returns>The string value of the NumberJ</returns>
    public override string ToString()
    {
        return _number;
    }

    /// <summary>
    /// Converts the NumberJ to a human readable string
    /// </summary>
    /// <returns>The human readable string value of the NumberJ</returns>
    public string HumanReadable()
    {
        return _number;
    }
}
