using System.Collections.Generic;

namespace ShowsService.Api.Model
{
    public class Show
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<CastMember> Cast { get; set; }
    }
}