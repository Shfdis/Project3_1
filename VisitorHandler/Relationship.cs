namespace VisitorHandler;
//AI_COMMENTS
/// <summary>
/// Represents a relationship between two visitors.
/// </summary>
public struct Relationship : IComparable<Relationship>
{
    /// <summary>
    /// The first visitor in the relationship.
    /// </summary>
    public readonly Visitor first, second;

    /// <summary>
    /// The type of the relationship.
    /// </summary>
    public readonly string type;

    /// <summary>
    /// Initializes a new instance of the <see cref="Relationship"/> struct with default values.
    /// </summary>
    public Relationship()
    {
        first = new Visitor();
        second = new Visitor();
        type = "";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Relationship"/> struct with specified visitors and data.
    /// </summary>
    /// <param name="visitors">The visitor group to find visitors from.</param>
    /// <param name="data">The relationship data.</param>
    /// <exception cref="Exception">Thrown when the relationship cannot be formed with the given data.</exception>
    public Relationship(VisitorGroup visitors, string data = "")
    {
        string[] parts = data.Split(".");
        try
        {
            type = parts[0] switch
            {
                "mistrust" => "недоверие",
                "befriend" => "дружба"
            };
            first = visitors.Filter("id", [parts[1]]).Visitors[0];
            second = visitors.Filter("id", [parts[2]]).Visitors[0];
        }
        catch (Exception)
        {
            throw new Exception("Нет такого отношения в текущей группе");
        }
        if (first.GetField("id").CompareTo(second.GetField("id")) == 1)
        {
            Visitor t = first;
            first = second;
            second = t;
        }
    }

    /// <summary>
    /// Determines whether two instances of <see cref="Relationship"/> are equal.
    /// </summary>
    /// <param name="a">The first relationship to compare.</param>
    /// <param name="b">The second relationship to compare.</param>
    /// <returns>true if the specified relationships are equal; otherwise, false.</returns>
    public static bool operator ==(Relationship a, Relationship b)
    {
        return a.type == b.type && a.first.GetField("id") == b.first.GetField("id") && a.second.GetField("id") == b.second.GetField("id");
    }

    /// <summary>
    /// Determines whether two instances of <see cref="Relationship"/> are not equal.
    /// </summary>
    /// <param name="a">The first relationship to compare.</param>
    /// <param name="b">The second relationship to compare.</param>
    /// <returns>true if the specified relationships are not equal; otherwise, false.</returns>
    public static bool operator !=(Relationship a, Relationship b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return type + '.' + first.GetField("id") + "." + second.GetField("id");
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates
    /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the
    /// other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(Relationship other)
    {
        return ToString().CompareTo(other.ToString());
    }

    /// <summary>
    /// Represents the relationship in a human-readable format.
    /// </summary>
    /// <returns>A string that represents the relationship.</returns>
    public string Represent()
    {
        string f = type.PadRight(20) + first.GetField("id")[1..^1].PadRight(20) + first.GetField("label")[1..^1].PadRight(20);
        string s = "".PadRight(20) + second.GetField("id")[1..^1].PadRight(20) + second.GetField("label")[1..^1].PadRight(20);
        return f + "\n" + s;
    }
}
