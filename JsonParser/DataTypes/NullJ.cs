namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// Represents a null JSON object
/// </summary>
public class NullJ : IJSON
{
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="s">The JSON string to parse</param>
    /// <param name="i">The index to start parsing from</param>
    public NullJ(string s = null, int i = 0)
    {
        if (!(i + 4 <= s.Length && s[i..(i + 4)] == "null"))
        {
            throw new FormatException("not a correct JSON format");
        }
    }
    /// <summary>
    /// Converts the object to a string
    /// </summary>
    /// <returns>The string representation of the object</returns>
    public override string ToString() => "null"; 
    /// <summary>
    /// Converts the object to a human-readable string
    /// </summary>
    /// <returns>The human-readable string representation of the object</returns>
    public string HumanReadable() => "null";
}
