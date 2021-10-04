using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Task2.Cmd.Models;

namespace Task2.Cmd.DataManager
{
    class JsonConverter
    {
        public List<T> Deserialize<T>(string text) where T : BaseEntity => JsonConvert.DeserializeObject<List<T>>(text);
        public string Serialize<T>(List<T> data) where T : BaseEntity => JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}
