using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShowsService.Data.Model;

namespace ShowsService.Data
{
    public interface IShowsRepository
    {
        Task<IEnumerable<Show>> GetShows(int pageNumber, short pageSize, CancellationToken cancellationToken);

        Task SaveShow(Show show, CancellationToken cancellationToken);
    }

    public class ShowsRepository : IShowsRepository
    {
        private readonly ShowsContext showsContext;

        public ShowsRepository(ShowsContext showsContext)
        {
            this.showsContext = showsContext;
        }

        public async Task<IEnumerable<Show>> GetShows(int pageNumber, short pageSize, CancellationToken cancellationToken)
        {
            return await this.showsContext
                             .Shows
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Include(x => x.Cast)
                             .ThenInclude(x => x.CastMember)
                             .ToListAsync(cancellationToken);
        }

        public async Task SaveShow(Show show, CancellationToken cancellationToken)
        {
            var castMembers = show.Cast.Select(x => x.CastMember).ToList();

            await showsContext.CastMembers
                              .UpsertRange(castMembers)
                              .NoUpdate()
                              .RunAsync(cancellationToken);

            foreach (var castMember in show.Cast)
            {
                castMember.CastMember = default;
            }

            showsContext.Shows.Add(show);

            await showsContext.SaveChangesAsync(cancellationToken);
        }
    }
}