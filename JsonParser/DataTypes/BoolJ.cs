namespace JsonParser;

public class BoolJ : IJSON
{
    private bool _value = false;

    public BoolJ()
    {
        
    }

    public BoolJ(string s, int index)
    {
        if (index + 4 <= s.Length && s[index .. (index + 4)] == "true")
        {
            _value = true;
        }
        else if (index + 5 <= s.Length && s[index .. (index + 5)] == "false")
        {
            _value = false;
        }
        else
        {
            throw new FormatException("invalid format");
        }
    }
    public bool Value => _value;
    public static implicit operator bool(BoolJ b)
    {
        return b.Value;
    }

    public static implicit operator BoolJ(bool b)
    {
        return new BoolJ { _value = b };
    }
}