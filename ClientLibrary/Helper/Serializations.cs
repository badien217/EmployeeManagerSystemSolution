using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientLibrary.Helper
{
    public static class Serializations
    {
        public static string SerializzeObj<T>(T modelObject) => JsonSerializer.Serialize(modelObject);
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString);
        public static IList<T> DeserializeJsonListString<T>(string jsonstring) => JsonSerializer.Deserialize < IList<T>>(jsonstring);

    }
}
