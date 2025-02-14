using VisitorHandler;
using JsonParser;
using System.Runtime.InteropServices;
namespace Project3_1;
//AI_COMMENTS
public class OutputHandler
{
    private VisitorGroup _group;
    /// <summary>
    /// Outputs the visitor group as a human-readable JSON string to the console.
    /// </summary>
    private void ConsoleOutput()
    {
        // Clear the console screen
        Console.Clear();
        
        // Output the visitor group's JSON representation in a human-readable format
        Console.WriteLine(_group.AsJson().HumanReadable());
    }

    /// <summary>
    /// Outputs the visitor group as a human-readable JSON string to a file.
    /// </summary>
    /// <remarks>
    /// The user is prompted to enter a path from the root of the project.
    /// The file will be written to the specified location.
    /// </remarks>
    private void FileOutput()
    {
        Console.Clear();
        Console.WriteLine("Введите путь от корня проекта");
        string fileSeparator = "\\";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) // define the OS
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
            JsonParser.JsonParser.WriteJson(_group.AsJson(), root + path);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
        /// <summary>
        /// Outputs a visitor group as a human-readable JSON string to either
        /// the console or a file.
        /// </summary>
        /// <param name="visitorGroup">The visitor group to output.</param>
        /// <remarks>
        /// The user is prompted to enter a choice of output method.
        /// </remarks>
    public OutputHandler(VisitorGroup visitorGroup)
    {
        Console.Clear();
        this._group = visitorGroup;
        Console.WriteLine("Введите 1 для вывода из консоли и 0 для файлового");
        int t;
        while (!int.TryParse(Console.ReadLine(), out t) || t < 0 || t > 1) ;
        if (t == 1)
        {
            ConsoleOutput();
        }
        else
        {
            FileOutput();
        }
    }
}