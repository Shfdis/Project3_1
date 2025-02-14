namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// A static class for parsing and writing json
/// </summary>
public static class JsonParser
{
    /// <summary>
    /// Writes the json to the file
    /// </summary>
    /// <param name="obj">The IJSONObject to write to the file</param>
    /// <param name="path">The path of the file to write to</param>
    public static void WriteJson(IJSONObject obj, string path)
    {
        TextWriter originalOutput = Console.Out;
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                Console.SetOut(writer);
                Console.WriteLine(obj.HumanReadable());
            }
        }
        catch (Exception)
        {
            Console.SetOut(originalOutput);
            throw;
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }

    /// <summary>
    /// Checks if the character is an escape sequence
    /// </summary>
    /// <param name="c">The character to check</param>
    /// <returns>True if the character is an escape sequence, false otherwise</returns>
    private static bool IsEscapeSequence(char c)
    {
        return c switch
        {
            '\\' => true,
            '"' => true,
            '\'' => true,
            '0' => true,
            'a' => true,
            'b' => true,
            't' => true,
            'n' => true,
            'r' => true,
            'f' => true,
            'e' => true,
            'v' => true,
            'u' => true,
            'U' => true,
            'x' => true,
            _ => false
        };
    }
    /// <summary>
    /// Skips the string symbol
    /// </summary>
    /// <param name="s">The string to skip the string symbol from</param>
    /// <param name="position">The position to skip the string symbol from</param>
    /// <returns>The string symbol skipped</returns>
    private static string SkipStringSymbol(string s, ref int position)
    {
        if (s[position] == '\\')
        {
            string ans = "\\";
            position++;
            if (position >= s.Length)
            {
                throw new FormatException("Неверный формат json");
            }

            if (!IsEscapeSequence(s[position]))
            {
                throw new FormatException("Неверный формат json");
            }
            ans += s[position];
            position++;
            return ans;
        } else {
            position++;
            return s[position - 1].ToString();
        }
    }
    /// <summary>
    /// Normalizes the string(deletes all space symbols and new line symbols)
    /// </summary>
    /// <param name="s">The string to normalize</param>
    /// <returns>The normalized string</returns>
    public static string Normalise(string s)
    {
        string answer = "";
        bool is_string = false;
        for (int i = 0; i < s.Length;)
        {
            char c = s[i];
            if (!is_string)
            {
                answer += c switch
                {
                    ' ' => "",
                    '\n' => "",
                    '\r' => "",
                    '\t' => "",
                    _ => c.ToString()
                };
                if (c == '"')
                {
                    is_string = true;
                }

                i++;
            }
            else
            {
                answer += SkipStringSymbol(s, ref i);
                if (c == '"')
                {
                    is_string = !is_string;
                }
            }
        }
        return answer;
    }

    /// <summary>
    /// Gets the state of the json
    /// </summary>
    /// <param name="s">The string to get the state of</param>
    /// <param name="i">The position to get the state of</param>
    /// <returns>The state of the json</returns>
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
    

    /// <summary>
    /// Parses the json
    /// </summary>
    /// <param name="s">The json to parse</param>
    /// <param name="norm">Whether to normalize the json</param>
    /// <param name="index">The position to start parsing from</param>
    /// <returns>The parsed json</returns>
    public static IJSON Parse(string? s, bool norm = false, int index = 0)
    {
        if (s is null)
        {
            throw new ArgumentNullException(nameof(s));
        }

        if (!norm)
        {
            s = Normalise(s);
        }
        switch (GetState(s, index))
        {
            case JsonStates.Object:
                return new Object(s, index);
            case JsonStates.Array:
                return new Array(s, index);
            case JsonStates.String:
                return new StringJ(s, index);
            case JsonStates.Boolean:
                return new BoolJ(s, index);
            case JsonStates.Number:
                return new NumberJ(s, index);
            default:
                return new NullJ(s, index);
        }
    }
    /// <summary>
    /// Reads the json from the file
    /// </summary>
    /// <param name="path">The path of the file to read from</param>
    /// <returns>The parsed json</returns>
    public static IJSONObject ReadJson(string path)
    {
        var standardInput = new StreamReader(Console.OpenStandardInput());
        IJSONObject obj;
        try
        {
            StreamReader input = new StreamReader(path);
            Console.SetIn(input);
            string json = Console.In.ReadToEnd();
            obj = Parse(json) as IJSONObject;
            input.Dispose();
        }
        catch (FormatException)
        {
            Console.SetIn(standardInput);
            throw new FormatException("Неверный формат json");
        }
        catch (InvalidCastException)
        {
            Console.SetIn(standardInput);
            throw new FormatException("Неверный формат json");
        }
        catch (Exception)
        {
            
            Console.SetIn(standardInput);
            throw new Exception("Невозможно прочитать из файла по этому пути");
        }
        Console.SetIn(standardInput);
        return obj;
    }
}
