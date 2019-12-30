using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

namespace ExplainingEveryString.Data
{
    public class JsonDataAccessor
    {
        public static JsonDataAccessor Instance { get; } = new JsonDataAccessor();

        private JsonSerializer serializer = new JsonSerializer()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Culture = CultureInfo.InvariantCulture,
            Formatting = Formatting.Indented,
            MissingMemberHandling = MissingMemberHandling.Error,
            DefaultValueHandling = DefaultValueHandling.Populate
        };

        public T Load<T>(String fileName)
        {
            using (var streamReader = new StreamReader(fileName))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        public void Save<T>(String fileName, T data)
        {
            using (var streamWriter = new StreamWriter(fileName))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
