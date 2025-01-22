namespace JsonParser;

public interface IJSONObject : IJSON
{
    IEnumerable<string> GetAllFields();
    string GetField(string fieldName);
    void SetField(string fieldName, string value); 
}