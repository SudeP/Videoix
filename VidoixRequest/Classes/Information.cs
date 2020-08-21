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
        public StatuModal Statu { get; set; }

        public partial class StatuModal
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

        public partial class InformationExt
        {
            public static Information FromJson(string json) => JsonConvert.DeserializeObject<Information>(json, Converter.Settings);
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
}
namespace Videoix
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CheckCoinsModal
    {
        [JsonProperty("statu")]
        public long Statu { get; set; }

        [JsonProperty("x")]
        public long X { get; set; }

        [JsonProperty("newX")]
        public long NewX { get; set; }

        [JsonProperty("watch_time")]
        public long WatchTime { get; set; }

        [JsonProperty("newTime")]
        public long NewTime { get; set; }

        [JsonProperty("isVideo")]
        public long IsVideo { get; set; }

        public partial class CheckCoinsModalExt
        {
            public static CheckCoinsModal FromJson(string json) => JsonConvert.DeserializeObject<CheckCoinsModal>(json, Converter.Settings);
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
}
