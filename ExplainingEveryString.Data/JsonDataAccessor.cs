using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

namespace ExplainingEveryString.Data
{
    internal class JsonDataAccessor
    {
        internal static JsonDataAccessor Instance { get; } = new JsonDataAccessor();

        private JsonSerializer serializer = new JsonSerializer()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Culture = CultureInfo.InvariantCulture,
            Formatting = Formatting.Indented,
            MissingMemberHandling = MissingMemberHandling.Error,
            DefaultValueHandling = DefaultValueHandling.Populate
        };

        internal T Load<T>(String fileName)
        {
            using (StreamReader streamReader = new StreamReader(GetPath(fileName)))
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        internal void Save<T>(String fileName, T data)
        {
            using (StreamWriter streamWriter = new StreamWriter(GetPath(fileName)))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, data);
            }
        }

        private String GetPath(String fileName)
        {
            return Path.Combine("Data", fileName);
        }
    }
}
