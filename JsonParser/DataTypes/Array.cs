namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// Represents a JSON array.
/// </summary>
public class Array : IJSON
{
    private List<IJSON> _children = new List<IJSON>();

    /// <summary>
    /// Gets the list of JSON children.
    /// </summary>
    public List<IJSON> Children
    {
        get
        {
            List<IJSON> children = new List<IJSON>();
            foreach (IJSON child in _children)
            {
                children.Add(child);
            }
            return children;
        }
    }

    /// <summary>
    /// Initializes a new instance of the Array class with the specified children.
    /// </summary>
    /// <param name="children">The list of JSON children.</param>
    public Array(List<IJSON> children)
    {
        _children = children;
    }

    /// <summary>
    /// Initializes a new empty instance of the Array class.
    /// </summary>
    public Array()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Array class by parsing a JSON string.
    /// </summary>
    /// <param name="s">The JSON string to parse.</param>
    /// <param name="leftBrace">The starting position of the left brace.</param>
    /// <exception cref="FormatException">Thrown when the JSON format is invalid.</exception>
    public Array(string s, int leftBrace = 0)
    {
        int i = leftBrace + 1;
        for (; i < s.Length && s[i] != ']';)
        {
            IJSON child = JsonParser.Parse(s, true, i);
            _children.Add(child);
            i += child.ToString().Length;
            if (s[i] == ',')
            {
                i++;
            }
        }

        if (i == s.Length)
        {
            throw new FormatException("Неверный формат json");
        }
    }

    /// <summary>
    /// Converts the array to its JSON string representation.
    /// </summary>
    /// <returns>The JSON string representation of the array.</returns>
    public override string ToString()
    {
        return "[" + string.Join(",", _children) + "]";
    }

    /// <summary>
    /// Converts the array to a human-readable JSON string representation.
    /// </summary>
    /// <returns>The human-readable JSON string representation of the array.</returns>
    public string HumanReadable()
    {
        string answer = "[\n";
        bool first = true;
        foreach (IJSON child in _children)
        {
            if (!first)
            {
                answer += ",\n";
            }
            first = false;
            answer += child.HumanReadable();
        }
        return answer + "\n]";
    }
}
