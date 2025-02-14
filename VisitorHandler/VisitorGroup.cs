using JsonParser;
using Array = JsonParser.Array;
//AI_COMMENTS
namespace VisitorHandler;

public class VisitorGroup(List<Visitor> visitors)
{
    private List<Visitor> _visitors = visitors;
    
    public bool Empty => _visitors.Count == 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisitorGroup"/> class.
    /// </summary>
    public VisitorGroup() : this(new List<Visitor>())
    {
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="VisitorGroup"/> class from the specified
    /// JSON object.
    /// </summary>
    /// <param name="json">The JSON object to initialize the visitor group from.</param>
    /// <exception cref="FormatException">The JSON object is not in the correct format.</exception>
    public VisitorGroup(IJSONObject json) : this(new List<Visitor>())
    {
        try
        {
            Array arr = new Array(json.GetField("elements"));
            foreach (IJSONObject child in arr.Children)
            {
                _visitors.Add(new Visitor(child));
            }

            GetAllRelationships();
        }
        catch (Exception)
        {
            throw new FormatException("Неверный формат json");
        }

    }
    public List<Visitor> Visitors
    {
        get
        {
            List<Visitor> result = new List<Visitor>();
            foreach (Visitor visitor in _visitors)
            {
                result.Add(visitor);
            }
            return result;
        }
    }

    /// <summary>
    /// Converts the visitor group to a JSON object.
    /// </summary>
    /// <returns>The JSON object representation of the visitor group.</returns>
    public IJSONObject AsJson()
    {
        JsonParser.Object jsonObject = new JsonParser.Object("{\"elements\":null}");
        List<IJSON> visitorsJson = new List<IJSON>();
        foreach (Visitor visitor in visitors)
        {
            visitorsJson.Add(visitor);
        }
        jsonObject.SetField("elements", (new Array(visitorsJson)).ToString());
        return jsonObject;
    }
    /// <summary>
    /// Checks if the field is sortable.
    /// </summary>
    /// <param name="field">The field to check.</param>
    /// <returns>True if the field is sortable, false otherwise.</returns>
    private bool IsSortable(string field)
    {
        string[] fields = [
            "id",
            "label",
            "description",
            "inherits",
            "lifetime",
        ];
        return fields.Contains(field);
    }
/// <summary>
/// Compares two visitors based on the specified field.
/// </summary>
/// <param name="a">The first visitor to compare.</param>
/// <param name="b">The second visitor to compare.</param>
/// <param name="field">The field to compare the visitors by.</param>
/// <returns>
/// An integer that indicates the relative order of the visitors:
/// less than zero if visitor 'a' is less than visitor 'b';
/// zero if they are equal; greater than zero if visitor 'a' is greater than visitor 'b'.
/// </returns>

    private int Compare(Visitor a, Visitor b, string field)
    {
        if (field == "lifetime")
        {
            return a.Lifetime.CompareTo(b.Lifetime);
        }
        return a.GetField(field).CompareTo(b.GetField(field));
    }
        /// <summary>
        /// Sorts the visitors in the group by the specified field.
        /// </summary>
        /// <param name="field">The field to sort the visitors by.</param>
        /// <returns>A new VisitorGroup with the sorted visitors.</returns>
        /// <exception cref="Exception">If the field is not sortable.</exception>
    public VisitorGroup Sort(string field)
    {
        if (!IsSortable(field))
        {
            throw new Exception($"Поле {field} недоступно для сортировки");
        }
        List<Visitor> visitors = Visitors;
        int Comparison(Visitor a, Visitor b) => Compare(a, b, field);
        visitors.Sort(Comparison);
        return new VisitorGroup(visitors);
    }

        /// <summary>
        /// Reverses the order of the visitors in the group.
        /// </summary>
    public void Reverse()
    {
        visitors.Reverse();
    }
        /// <summary>
        /// Filters the visitors in the group by the specified field and list of values.
        /// </summary>
        /// <param name="field">The field to filter the visitors by.</param>
        /// <param name="values">The list of values to filter by.</param>
        /// <returns>A new VisitorGroup with the filtered visitors.</returns>
        /// <exception cref="Exception">If the field is not sortable or if the field is null.</exception>
    public VisitorGroup Filter(string field, List<string> values)
    {
        if (!IsSortable(field))
        {
            throw new Exception($"Поле {field} недоступно для фильтрации");
        }

        List<Visitor> visitors = new List<Visitor>();

        foreach (Visitor visitor in _visitors)
        {
            string? current = visitor.GetField(field);

            if (field != "lifetime" && current is not null) // lifetime -  это число
            {
                current = current[1..^1];
            }

            if (values.Contains(current))
            {
                visitors.Add(visitor);
            }
        }

        return new VisitorGroup(visitors);
     } 

        /// <summary>
        /// Returns all relationships between visitors in the group.
        /// </summary>
        /// <returns>A list of relationships.</returns>
        /// <remarks>
        /// If a visitor has multiple relationships with the same visitor, only one of them is returned.
        /// </remarks>
    public List<Relationship> GetAllRelationships()
    {
        List<Relationship> result = new List<Relationship>();
        foreach (Visitor visitor in Visitors)
        {
            if (visitor.Xexts is not null)
            {
                foreach (string relationship in visitor.Xexts.GetAllFields())
                {
                    try
                    {
                        result.Add(new Relationship(this, relationship));
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }
        result.Sort(); // сортируем
        List<Relationship> withoutEqual = new List<Relationship>();
        foreach (Relationship relationship in result) // этот цикл удаляет одинаковые отношения
        {
            if (withoutEqual.Count == 0)
            {
                withoutEqual.Add(relationship);
            } else if (withoutEqual[^1] != relationship)
            {
                withoutEqual.Add(relationship);
            }
        }
        return withoutEqual;
    }

/// <summary>
/// Merges the current visitor group with another visitor group.
/// </summary>
/// <param name="other">The other visitor group to merge with.</param>
/// <returns>A new VisitorGroup containing all visitors from both groups.</returns>

    public VisitorGroup Merge(VisitorGroup other)
    {
        List<Visitor> result = new List<Visitor>();
        foreach (Visitor visitor in other.Visitors)
        {
            result.Add(visitor);
        }
        foreach (Visitor visitor in Visitors)
        {
            result.Add(visitor);
        }
        return new VisitorGroup(result);
    }
}
