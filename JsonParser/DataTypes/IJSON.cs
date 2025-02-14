namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// Interface representing a JSON object
/// </summary>
public interface IJSON
{
    /// <summary>
    /// Converts the JSON object to its string representation
    /// </summary>
    /// <returns>The string representation of the JSON object</returns>
    string ToString();
    
    /// <summary>
    /// Converts the JSON object to a human-readable string representation
    /// </summary>
    /// <returns>The human-readable string representation of the JSON object</returns>
    string HumanReadable();
}
