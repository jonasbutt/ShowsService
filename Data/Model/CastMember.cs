using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShowsService.Data.Model
{
    public class CastMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? Birthday { get; set; }

        public ICollection<ShowCastMember> Shows { get; set; }
    }
}