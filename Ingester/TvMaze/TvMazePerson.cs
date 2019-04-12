using System;
using Newtonsoft.Json;
using ShowsService.Tools.Serialization;

namespace ShowsService.Ingester.TvMaze
{
    public class TvMazePerson
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(CustomFormatDateTimeConverter), "yyyy-MM-dd")]
        public DateTimeOffset? Birthday { get; set; }
    }
}