using JsonParser;
using Array = JsonParser.Array;
//AI_COMMENTS
namespace VisitorHandler;

/// <summary>
/// Represents the Xtriggers class that implements the IJSONObject interface.
/// </summary>
public class Xtriggers : IJSONObject
{
    private IJSONObject json;

    /// <summary>
    /// Initializes a new instance of the Xtriggers class with a null JSON object.
    /// </summary>
    public Xtriggers()
    {
        json = null;
    }

    /// <summary>
    /// Initializes a new instance of the Xtriggers class with the specified JSON object.
    /// </summary>
    /// <param name="json">The JSON object to initialize the Xtriggers with.</param>
    public Xtriggers(IJSONObject json)
    {
        this.json = json;
    }

    /// <summary>
    /// Gets all field names from the JSON object.
    /// </summary>
    /// <returns>An enumerable of all the field names.</returns>
    public IEnumerable<string> GetAllFields()
    {
        return json.GetAllFields();
    }

    /// <summary>
    /// Gets the value of the specified field.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>The value of the field.</returns>
    public string GetField(string fieldName)
    {
        return json.GetField(fieldName);
    }

    /// <summary>
    /// Sets the value of the specified field.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <param name="value">The value to set.</param>
    public void SetField(string fieldName, string value)
    {
        json.SetField(fieldName, value);
    }

    /// <summary>
    /// Converts the JSON object to a human-readable string.
    /// </summary>
    /// <returns>A human-readable string representation of the JSON object.</returns>
    public string HumanReadable() => json.HumanReadable();
}
