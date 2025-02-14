namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// Represents a boolean value in a JSON object
/// </summary>
public class BoolJ : IJSON
{
    private bool _value = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoolJ"/> class
    /// </summary>
    public BoolJ()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BoolJ"/> class
    /// </summary>
    /// <param name="s">The string to parse</param>
    /// <param name="index">The index to start parsing at</param>
    public BoolJ(string s, int index)
    {
        if (index + 4 <= s.Length && s[index .. (index + 4)] == "true")
        {
            _value = true;
        }
        else if (index + 5 <= s.Length && s[index .. (index + 5)] == "false")
        {
            _value = false;
        }
        else
        {
            throw new FormatException("invalid format");
        }
    }

    /// <summary>
    /// Gets the value of the boolean
    /// </summary>
    public bool Value => _value;

    /// <summary>
    /// Implicitly casts the boolean to a <see cref="BoolJ"/>
    /// </summary>
    /// <param name="b">The boolean to cast</param>
    /// <returns>The casted boolean</returns>
    public static implicit operator bool(BoolJ b)
    {
        return b.Value;
    }

    /// <summary>
    /// Gets the value of the boolean as a string
    /// </summary>
    /// <returns>The value of the boolean as a string</returns>
    public string HumanReadable()
    {
        return _value ? "true" : "false";
    }

    /// <summary>
    /// Implicitly casts the boolean to a <see cref="BoolJ"/>
    /// </summary>
    /// <param name="b">The boolean to cast</param>
    /// <returns>The casted boolean</returns>
    public static implicit operator BoolJ(bool b)
    {
        return new BoolJ { _value = b };
    }
}
