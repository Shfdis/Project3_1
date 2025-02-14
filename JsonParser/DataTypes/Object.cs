
namespace JsonParser;
//AI_COMMENTS
public class Object : IJSONObject
{
    private Dictionary<string, IJSON> _children = new Dictionary<string, IJSON>();
    /// <summary>
    /// A constructor that creates an empty json object
    /// </summary>
    
    public Object()
    {
        
    }
    /// <summary>
    /// A constructor that creates a json object from a string
    /// </summary>
    /// <param name="s">The string to parse</param>
    /// <param name="leftBrace">The index of the left brace</param>
    public Object(string s, int leftBrace = 0)
    {
        int i = leftBrace + 1;
        for (; i < s.Length && s[i] != '}';)
        {
            string key = new StringJ(s, i);
            i += key.Length + 3;
            IJSON child = JsonParser.Parse(s, true, i);
            _children.Add(key, child);
            i += child.ToString().Length;
            if (s[i] == ',')
            {
                i++;
            }
        }

        if (i == s.Length)
        {
            throw new FormatException("Not a valid JSON");
        }
    }
    /// <summary>
    /// Gets all fields of the object
    /// </summary>
    /// <returns>An enumerable of all the field names</returns>
    public IEnumerable<string> GetAllFields()
    {
        List<string> result = new List<string>();
        foreach (string key in _children.Keys)
        {
            result.Add(key);
        }

        return result;
    }

    /// <summary>
    /// Gets the value of the field
    /// </summary>
    /// <param name="fieldName">The name of the field</param>
    /// <returns>The value of the field or null if it doesn't exist</returns>
    public string? GetField(string fieldName)
    {
        if (!_children.ContainsKey(fieldName))
        {
            return null;
        }
        return _children[fieldName].ToString();
    }

    /// <summary>
    /// Sets the value of the field
    /// </summary>
    /// <param name="fieldName">The name of the field</param>
    /// <param name="value">The value of the field</param>
    public void SetField(string fieldName, string value)
    {
        if (!_children.ContainsKey(fieldName))
        {
            throw new KeyNotFoundException($"Нет такого ключа");
        }
        IJSON valueJSON = JsonParser.Parse(value);
        _children[fieldName] = valueJSON;
    }

    /// <summary>
    /// Converts the object to a string
    /// </summary>
    /// <returns>The string representation of the object</returns>
    public override string ToString()
    {
        string answer = "{";
        bool first = true;
        foreach (string key in _children.Keys)
        {
            if (!first)
            {
                answer += ",";
            }
            answer += $"\"{key}\"" + ":" + _children[key].ToString();
            first = false;
        }
        return answer + "}";
    }

    /// <summary>
    /// Converts the object to a human-readable string
    /// </summary>
    /// <returns>The human-readable string representation of the object</returns>
    public string HumanReadable()
    {
        string answer = "{\n";
        bool first = true;
        foreach (string key in _children.Keys)
        {
            if (!first)
            {
                answer += ",\n";
            }
            answer += $"\"{key}\"" + ":" + _children[key].HumanReadable();
            first = false;
        }
        return answer + "\n}";
    }
}
