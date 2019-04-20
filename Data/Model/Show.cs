using System.Collections.Generic;

namespace ShowsService.Data.Model
{
    public class Show
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IReadOnlyCollection<CastMember> Cast { get; set; }

        public override string ToString() => $"{this.Id} {this.Name}";
    }
}