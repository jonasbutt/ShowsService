namespace ShowsService.Data.Model
{
    public class ShowCastMember
    {
        public long ShowId { get; set; }

        public Show Show { get; set; }

        public long CastMemberId { get; set; }

        public CastMember CastMember { get; set; }
    }
}