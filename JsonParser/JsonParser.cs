namespace JsonParser;

public static class JsonParser
{
    public static void WriteJson(IJSONObject obj, string path)
    {
        StreamWriter sw = new StreamWriter(path);
        sw.WriteLine(obj);
        sw.Flush();
        sw.Close();
    }
    public static string Normalise(string s)
    {
        string answer = "";
        foreach (char c in s)
        {
            answer += c switch
            {
                ' ' => "",
                '\n' => "",
                _ => c.ToString()
            };
        }
        return answer;
    }

    private static JsonStates GetState(string s, int i)
    {
        return s[i] switch
        {
            '{' => JsonStates.Object,
            '[' => JsonStates.Array,
            '"' => JsonStates.String,
            't' => JsonStates.Boolean,
            'f' => JsonStates.Boolean,
            'n' => JsonStates.Null,
            _ => JsonStates.Number
        };
    }

    private static bool CheckNull(string s, int i)
    {
        if (i + 4 <= s.Length)
        {
            return s[i..(i + 4)] == "null";
        }

        return false;
    }

    public static IJSON Parse(string? s, bool normalized = false, int index = 0)
    {
        if (s is null)
        {
            throw new ArgumentNullException(nameof(s));
        }
        switch (GetState(s, index))
        {
            case JsonStates.Object:
                return new Object(s, normalized, index);
            case JsonStates.Array:
                return new Array(s, normalized, index);
            case JsonStates.String:
                return new StringJ(s, index);
            case JsonStates.Boolean:
                return new BoolJ(s, index);
            case JsonStates.Number:
                return new NumberJ(s, index);
            default:
                return CheckNull(s, index) ? null : throw new InvalidDataException("Invalid JSON format.");
        }
    }
    public static IJSONObject ReadJson(string path)
    {
        Stream input = new FileStream(path, FileMode.Open);
        string json = new StreamReader(input).ReadToEnd();
        IJSON obj = Parse(json);
        if (obj is not IJSONObject jsonObject)
        {
            throw new FormatException("Not an object");
        }
        return jsonObject;
    }
}