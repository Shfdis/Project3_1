namespace JsonParser;
//AI_COMMENTS
/// <summary>
/// States of json value
/// </summary>
internal enum JsonStates
{
    Object,
    Array,
    String,
    Number,
    Boolean,
    Null
}