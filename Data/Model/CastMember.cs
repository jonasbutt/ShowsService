using System;

namespace ShowsService.Data.Model
{
    public class CastMember
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? Birthday { get; set; }
    }
}