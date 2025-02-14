namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// Represents an object in the JSON
/// </summary>
public interface IJSONObject : IJSON
{
    /// <summary>
    /// Gets all the fields in the JSON object
    /// </summary>
    /// <returns>Collection of strings representing the names of the fields</returns>
    IEnumerable<string> GetAllFields();

    /// <summary>
    /// Gets the value of a field in the JSON object
    /// </summary>
    /// <param name="fieldName">The name of the field</param>
    /// <returns>The value of the field</returns>
    string GetField(string fieldName);

    /// <summary>
    /// Sets the value of a field in the JSON object
    /// </summary>
    /// <param name="fieldName">The name of the field</param>
    /// <param name="value">The value of the field</param>
    void SetField(string fieldName, string value);
}
