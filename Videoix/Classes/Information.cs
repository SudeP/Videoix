namespace Videoix
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Information
    {
        [JsonProperty("statu")]
        public Statu Statu { get; set; }
    }

    public partial class Statu
    {
        [JsonProperty("0")]
        public string The0 { get; set; }

        [JsonProperty("1")]
        public string The1 { get; set; }

        [JsonProperty("2")]
        public string The2 { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("coins")]
        public string Coins { get; set; }

        [JsonProperty("account_balance")]
        public string AccountBalance { get; set; }
    }

    public partial class Information
    {
        public static Information FromJson(string json) => JsonConvert.DeserializeObject<Information>(json, Videoix.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Information self) => JsonConvert.SerializeObject(self, Videoix.Converter.Settings);
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
}
