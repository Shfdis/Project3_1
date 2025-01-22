namespace JsonParser;

public struct StringJ : IJSON
{
    string _value;

    public StringJ()
    {
        _value = "";
    }
    public StringJ(string s, int index = 0)
    {
        if (s[index] != '"')
        {
            throw new FormatException("is not a string");
        }

        int i = index + 1;
        _value = "";
        for (; i < s.Length && s[i] != '"'; i++)
        {
            _value += s[i];
        }

        if (i == s.Length)
        {
            throw new FormatException("is not a string");
        }
    }
    string Value => _value;
    public static implicit operator string(StringJ s)
    {
        return s.Value;
    }

    public static implicit operator StringJ(string s)
    {
        return new StringJ { _value = s };
    }

    public override string ToString()
    {
        return '"' + _value + '"';
    }
}