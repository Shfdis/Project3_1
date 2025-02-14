using JsonParser;
using VisitorHandler;
using System.Runtime.InteropServices;
//AI_COMMENTS
namespace Project3_1;

public class InputHandler
{
    private IJSONObject _json;
    public VisitorGroup VisitorGroup => new VisitorGroup(_json);
    /// <summary>
    /// Reads a JSON from the console input and parses it. If the parsing fails,
    /// throws an exception with the message of the exception.
    /// </summary>
    private void ConsoleInput()
    {
        Console.Clear();
        Console.WriteLine("Вводите json, для завершения введите строку end");
        string line = "";
        string full = "";
        do
        {
            full += line;
            line = Console.ReadLine();
        } while (line != "end");

        try
        {
            _json = JsonParser.JsonParser.Parse(full) as IJSONObject;
        }
        catch (InvalidCastException)
        {
            throw new FormatException("Неверный формат json");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }

    /// <summary>
    /// Reads a JSON from the file and parses it. If the parsing fails,
    /// throws an exception with the message of the exception.
    /// </summary>
    public void FileInput()
    {
        Console.Clear();
        Console.WriteLine("Введите путь от корня проекта");
        string fileSeparator = "\\";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            fileSeparator = "/";
        }

        string root = "";
        for (int i = 0; i < 4; i++)
        {
            root += ".." + fileSeparator;
        }
        string path = Console.ReadLine();
        try
        {
            _json = JsonParser.JsonParser.ReadJson(root + path);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
        /// <summary>
        /// Constructor for InputHandler.
        /// Prompts the user for a way of input (console or file) and reads the JSON.
        /// If the parsing fails, throws an exception with the message of the exception.
        /// </summary>
    public InputHandler()
    {
        Console.Clear();
        Console.WriteLine("Введите 1 для ввода из консоли и 0 для файлового");
        int t;
        while (!int.TryParse(Console.ReadLine(), out t) || t < 0 || t > 1) ;
        if (t == 1)
        {
            ConsoleInput();
        }
        else
        {
            FileInput();
        }
    }
}