using JsonParser;
using static JsonParser.JsonParser;
//AI_COMMENTS
namespace VisitorHandler
{

    /// <summary>
    /// Represents a visitor.
    /// </summary>
    public struct Visitor : IJSONObject
    {
        private static string[] allFields =
            ["id", "label", "desc", "inherits", "aspects", "decayto", "lifetime", "xtriggers", "xexts"];
        private int _lifetime = -1;
        /// <summary>
        /// Gets the lifetime of the visitor.
        /// </summary>
        public int Lifetime => _lifetime;
        private IJSONObject _xexts;
        /// <summary>
        /// Gets the xexts of the visitor.
        /// </summary>
        public IJSONObject Xexts => _xexts;
        private readonly IJSONObject _asJsonObject;

        private void Check()
        {
            string[] strings = ["id", "label", "desc", "inherits", "decayto"];
            foreach (string s in strings) 
            {
                if (_asJsonObject.GetField(s) is not null)
                {
                    if (_asJsonObject.GetField(s)[0] != '"')
                    {
                        throw new FormatException("Invalid JSON format");
                    }
                }
            }
        }
        private void Update()
        {
            try
            {
                string? s = _asJsonObject.GetField("lifetime");
                if (s is not null)
                {
                    _lifetime = int.Parse(s);
                }
                string value = _asJsonObject.GetField("xexts");
                if (value is not null)
                {
                    _xexts = Parse(value) as IJSONObject;
                }
                else
                {
                    _xexts = null;
                }
            }
            catch (Exception)
            {
                throw new FormatException("Invalid JSON format");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Visitor"/> struct.
        /// </summary>
        public Visitor()
        {
            _lifetime = -1;
            _xexts = null;
            _asJsonObject = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Visitor"/> struct from the specified
        /// JSON object.
        /// </summary>
        /// <param name="jsonObject">The JSON object to initialize the visitor from.</param>
        public Visitor(IJSONObject jsonObject)
        {
            _asJsonObject = jsonObject;
            IEnumerable<string> fields = jsonObject.GetAllFields();
            foreach (string field in fields)
            {
                if (!allFields.Contains(field))
                {
                    throw new FormatException("Invalid data");
                }
            }

            if (_asJsonObject.GetField("aspects") is not null)
            {
                new Aspects(Parse(_asJsonObject.GetField("aspects")) as IJSONObject);
            }

            if (_asJsonObject.GetField("xtriggers") is not null)
            {
                new Xtriggers(Parse(_asJsonObject.GetField("xtriggers")) as IJSONObject);
            }
            Update();
            Check();
        }

        /// <summary>
        /// Gets all the field names from the JSON object.
        /// </summary>
        /// <returns>An enumerable of all the field names.</returns>
        public IEnumerable<string> GetAllFields()
        {
            return _asJsonObject.GetAllFields();
        }

        /// <summary>
        /// Gets the value of the field with the specified name.
        /// </summary>
        /// <param name="key">The name of the field.</param>
        /// <returns>The value of the field.</returns>
        public string GetField(string key)
        {
            return _asJsonObject.GetField(key);
        }

        /// <summary>
        /// Sets the value of the field with the specified name.
        /// </summary>
        /// <param name="key">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        public void SetField(string key, string value)
        {
            _asJsonObject.SetField(key, value);
            Update();
        }

        /// <summary>
        /// Converts the visitor to a string.
        /// </summary>
        /// <returns>The string representation of the visitor.</returns>
        public override string ToString()
        {
            return _asJsonObject.ToString();
        }
        
        /// <summary>
        /// Converts the visitor to a human-readable string.
        /// </summary>
        /// <returns>The human-readable string representation of the visitor.</returns>
        public string HumanReadable() => _asJsonObject.HumanReadable();
    }
}