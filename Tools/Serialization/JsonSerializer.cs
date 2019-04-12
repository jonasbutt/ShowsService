using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ShowsService.Tools.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object @object);

        TObject Deserialize<TObject>(string json);

        IDictionary<string, string> SerializeToDictionary(object @object);
    }

    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings defaultSettings;
        private readonly Newtonsoft.Json.JsonSerializer jsonSerializer;

        public JsonSerializer()
        {
            this.defaultSettings =
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };

            this.jsonSerializer = Newtonsoft.Json.JsonSerializer.Create(this.defaultSettings);
        }

        public string Serialize(object @object)
        {
            return JsonConvert.SerializeObject(@object, this.defaultSettings);
        }

        public TObject Deserialize<TObject>(string json)
        {
            return JsonConvert.DeserializeObject<TObject>(json, this.defaultSettings);
        }

        public IDictionary<string, string> SerializeToDictionary(object @object)
        {
            var jObject = JObject.FromObject(@object, this.jsonSerializer);
            var jTokenDictionary = (IDictionary<string, JToken>) jObject;
            return jTokenDictionary.ToDictionary(keyValuePair => keyValuePair.Key,
                                                 keyValuePair => keyValuePair.Value.Value<string>());
        }
    }
}
