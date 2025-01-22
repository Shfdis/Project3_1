
namespace JsonParser;

public class Object : IJSONObject
{
    private Dictionary<string, IJSON> _children = new Dictionary<string, IJSON>();
    public Object(string s, bool normalised = false, int leftBrace = 0)
    {
        if (!normalised)
        {
            s = JsonParser.Normalise(s);
            normalised = true;
        }

        int i = leftBrace + 1;
        for (; i < s.Length && s[i] != '}';)
        {
            string key = new StringJ(s, i);
            i += key.Length + 3;
            IJSON child = JsonParser.Parse(s, normalised, i);
            _children.Add(key, child);
            i += child.ToString().Length;
        }

        if (i == s.Length)
        {
            throw new FormatException("Not a valid JSON");
        }
    }
    public IEnumerable<string> GetAllFields()
    {
        List<string> result = new List<string>();
        foreach (string key in _children.Keys)
        {
            result.Add(key);
        }

        return result;
    }

    public string? GetField(string fieldName)
    {
        if (!_children.ContainsKey(fieldName))
        {
            return null;
        }
        return _children[fieldName].ToString();
    }

    public void SetField(string fieldName, string value)
    {
        IJSON valueJSON = JsonParser.Parse(value);
        _children[fieldName] = valueJSON;
    }

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
            answer += key + ":" + _children[key].ToString();
        }
        return answer;
    }
}