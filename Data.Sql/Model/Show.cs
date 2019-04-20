using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShowsService.Data.Sql.Model
{
    public class Show
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<ShowCastMember> Cast { get; set; }

        public override string ToString() => $"{this.Id} {this.Name}";
    }
}