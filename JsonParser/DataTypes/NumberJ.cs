namespace JsonParser;

public class NumberJ : IJSON
{
    private double _number;

    public NumberJ()
    {
        _number = 0;
    } 
    public double Number => _number;
    public static implicit operator double(NumberJ number)
    {
        return number.Number;
    }

    public static implicit operator NumberJ(double number)
    {
        return new NumberJ { _number = number };
    }

    public NumberJ(string s, int index)
    {
        string ans = "";
        while (index < s.Length && (ans == "" || double.TryParse(ans + s[index], out _number)))
        {
            ans += s[index++];
        }

        if (!double.TryParse(ans, out _number))
        {
            throw new FormatException("Invalid format");
        }
    }
}