namespace JsonParser;

public class Array : IJSON
{
    private List<IJSON> _children = new List<IJSON>();

    public Array()
    {
    }

    public Array(string s, bool normalised = false, int leftBrace = 0)
    {
        if (!normalised)
        {
            s = JsonParser.Normalise(s);
            normalised = true;
        }

        int i = leftBrace + 1;
        for (; i < s.Length && s[i] != ']';)
        {
            IJSON child = JsonParser.Parse(s, normalised, i);
            _children.Add(child);
            i += child.ToString().Length + 1;
        }

        if (i == s.Length)
        {
            throw new FormatException("Not a valid JSON");
        }
    }
    public override string ToString()
    {
        return "[" + string.Join(", ", _children) + "]";
    }
}