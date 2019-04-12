using System;
using Newtonsoft.Json;
using ShowsService.Tools.Serialization;

namespace ShowsService.Api.Model
{
    public class CastMember
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(CustomFormatDateTimeConverter), "yyyy-MM-dd")]
        public DateTimeOffset? Birthday { get; set; }
    }
}