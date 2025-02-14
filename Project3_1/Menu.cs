using VisitorHandler;
using Graphics;
using System.Runtime.InteropServices;
namespace Project3_1;
//AI_COMMENTS
public static class Menu
{
    private static VisitorGroup _currentGroup = new VisitorGroup();
    private enum Option
    {
        Input = 0,
        Filter = 1,
        Sort = 2,
        Relationships = 3,
        Graph = 4,
        Output = 5,
        Exit = 6,
        NotAnOption = 7
    }
    private static readonly string[] _options =
    [
        "Ввести данные",
        "Отфильтровать данные",
        "Отсортировать данные",
        "Вывести отношения",
        "Вывести граф отношений",
        "Вывести данные",
        "Выход"
    ];

/// <summary>
/// Pauses the program execution and prompts the user to press enter to continue.
/// </summary>

    private static void Wait()
    {
        Console.WriteLine("Нажмите enter чтобы продолжить");
        Console.ReadLine();
    }

/// <summary>
/// Displays the menu options to the console, clearing any previous output.
/// </summary>

    private static void PrintMenu()
    {
        Console.Clear();
        for (int i = 0; i < _options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {_options[i]}");
        }
    }
    /// <summary>
    /// Displays a list of possible fields that can be filtered or sorted by.
    /// </summary>
    private static void ShowFields() {
        string[] fields = [
            "id",
            "label",
            "description",
            "inherits",
            "lifetime",
        ];
        foreach (string field in fields)
        {
            Console.WriteLine(field);
        }
    }
/// <summary>
/// Prompts the user to input a field and values to filter the current visitor group.
/// </summary>
/// <remarks>
/// The user is asked to select a field from a list of sortable fields and enter values for filtering.
/// If the field is valid and sortable, the visitor group is filtered using the provided values.
/// If an exception occurs, an error message is displayed, and the exception is thrown.
/// </remarks>

    private static void Filter()
    {
        Console.Clear();
        Console.WriteLine("Введите поле для фильтрации среди полей");
        ShowFields();
        string field = "";
        string[] possible = [
            "id",
            "label",
            "description",
            "inherits",
            "lifetime",
        ];
        while (!possible.Contains(field))
        {
            field = Console.ReadLine();
        }
        
        try
        {
            Console.WriteLine("Напишите количество значений для фильтрации");
            int n;
            while (!int.TryParse(Console.ReadLine(), out n) || n < 1) ;
            
            Console.WriteLine("Напишите значения для фильтрации");
            List<string> fields = new List<string>();
            for (int i = 0; i < n; i++)
            {
                string value = Console.ReadLine();
                fields.Add(value);
            }
            _currentGroup = _currentGroup.Filter(field, fields);
        }
        catch (Exception)
        {
            Console.WriteLine("Поле недоступно для фильтрации по нему");
            throw;
        }
    }

/// <summary>
/// Reads user input from the console and converts it to a menu option.
/// </summary>
/// <returns>
/// A valid <see cref="Option"/> if the input is an integer between 1 and 7, inclusive; 
/// otherwise, returns <see cref="Option.NotAnOption"/>.
/// </returns>

    private static Option ReadOption()
    {
        string s = Console.ReadLine();
        if (int.TryParse(s, out int h) && h is >= 1 and <= 7)
        {
            return (Option)(h - 1);
        }
        return Option.NotAnOption;
    }

/// <summary>
/// Prompts the user to input a field to sort the current visitor group.
/// </summary>
/// <remarks>
/// If the field is valid and sortable, the visitor group is sorted by the provided field.
/// If an exception occurs, an error message is displayed.
/// </remarks>

    private static void Sort()
    {
        Console.WriteLine("Введите поле, по которому нужно осуществить сортировку");
        try
        {
            string field = Console.ReadLine();
            _currentGroup = _currentGroup.Sort(field);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

/// <summary>
/// Checks if the current visitor group is empty and displays a message if it is.
/// </summary>
/// <returns>
/// True if the current visitor group is not empty; otherwise, false.
/// </returns>

    private static bool Check()
    {
        if (_currentGroup.Empty)
        {
            Console.WriteLine("Сначала введите данные");
        }
        return !_currentGroup.Empty;
    }
        /// <summary>
        /// Runs the program.
        /// </summary>
        /// <remarks>
        /// This method runs the main loop of the program. It prints the menu, reads user input, and performs the corresponding action.
        /// The actions include sorting, filtering, outputting, and saving the visitor group to a file.
        /// If an exception occurs while performing an action, the error message is displayed.
        /// The loop continues until the user chooses to exit.
        /// </remarks>
    public static void Run()
    {
        Option option = Option.NotAnOption;
        do
        {
            PrintMenu();
            option = ReadOption();
            while (option == Option.NotAnOption)
            {
                Console.WriteLine("Нет такой функции");
                option = ReadOption();
            }

            if (option == Option.Sort && Check())
            {
                Sort();
            }
            else if (option == Option.Input)
            {
                try
                {
                    InputHandler input = new InputHandler();
                    _currentGroup = input.VisitorGroup;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            } else if (option == Option.Output && Check())
            {
                if (_currentGroup.Empty)
                {
                    Console.WriteLine("Нет данных");
                }
                else
                {
                    try
                    {
                        new OutputHandler(_currentGroup);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else if (option == Option.Relationships && Check())
            {
                foreach (Relationship relationship in _currentGroup.GetAllRelationships())
                {
                    Console.WriteLine(relationship.Represent());
                }
            } else if (option == Option.Filter && Check())
            {
                Filter();
            } 
            else if (option == Option.Graph && Check())
            {
                RelationshipDrawer drawer = new RelationshipDrawer(_currentGroup.GetAllRelationships());
                drawer.Draw(); //draws the graph
                Console.Clear();
                Console.WriteLine("Введите путь, куда сохранить граф от корня проекта");
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
                path = root + fileSeparator + path;
                try
                {
                    drawer.SaveImage(path); // save the image
                }
                catch (Exception)
                {
                    Console.WriteLine("Проблемы с графом");
                }
            }
            Wait();
        } while (option != Option.Exit);
    }
}