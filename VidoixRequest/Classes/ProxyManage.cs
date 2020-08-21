using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Videoix.Classes
{
    public class ProxyManage
    {
        public Proxy11 IpResult { get; set; }
        public void New()
        {
            var ipClient = new RestClient("https://proxy11.com/api/proxy.json?key=MTU4NA.Xxwc0g.8A1eCSmbVwHAf92VJLr4A-wk3iA");
            var ipRequest = new RestRequest();
            var ipResponse = ipClient.Execute(ipRequest);
            IpResult = JsonConvert.DeserializeObject<Proxy11>(ipResponse.Content);
        }
    }
    public partial class Proxy11
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("google")]
        public long Google { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Port { get; set; }

        [JsonProperty("time")]
        public decimal Time { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
    }

    public partial class Proxy11
    {
        public static Proxy11 FromJson(string json) => JsonConvert.DeserializeObject<Proxy11>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Proxy11 self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

}
