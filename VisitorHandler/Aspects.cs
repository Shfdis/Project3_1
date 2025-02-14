using JsonParser;

namespace VisitorHandler;
//AI_COMMENTS
/// <summary>
/// Represents an Aspects class that implements IJSONObject interface.
/// </summary>
public class Aspects : IJSONObject
{
    /// <summary>
    /// The underlying JSON object.
    /// </summary>
    IJSONObject jsonObject;

    /// <summary>
    /// Gets all field names from the JSON object.
    /// </summary>
    /// <returns>An enumerable of all the field names.</returns>
    public IEnumerable<string> GetAllFields() => jsonObject.GetAllFields();

    /// <summary>
    /// Gets the value of the specified field.
    /// </summary>
    /// <param name="field">The name of the field.</param>
    /// <returns>The value of the field.</returns>
    public string GetField(string field) => jsonObject.GetField(field);

    /// <summary>
    /// Sets the value of the specified field.
    /// </summary>
    /// <param name="field">The name of the field.</param>
    /// <param name="value">The value to set.</param>
    public void SetField(string field, string value) => jsonObject.SetField(field, value);

    /// <summary>
    /// Initializes a new instance of the Aspects class.
    /// </summary>
    public Aspects() { }

    /// <summary>
    /// Converts the JSON object to a human-readable string.
    /// </summary>
    /// <returns>A human-readable string representation of the JSON object.</returns>
    public string HumanReadable() => jsonObject.HumanReadable();

    /// <summary>
    /// Initializes a new instance of the Aspects class with a specified JSON object.
    /// </summary>
    /// <param name="jsonObject">The JSON object to initialize with.</param>
    /// <exception cref="Exception">Thrown when a field value is not a double.</exception>
    public Aspects(IJSONObject jsonObject)
    {
        this.jsonObject = jsonObject;
        foreach (string field in jsonObject.GetAllFields())
        {
            if (!double.TryParse(GetField(field), out _))
            {
                throw new Exception($"Field {field} is not a double");
            }
        }
    }
}
