using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShowsService.Data.Model;

namespace ShowsService.Data.Sql
{
    public class ShowsRepository : IShowsRepository
    {
        private readonly ShowsContext showsContext;

        public ShowsRepository(ShowsContext showsContext)
        {
            this.showsContext = showsContext;
        }

        public async Task<IEnumerable<Show>> GetShows(int pageNumber, short pageSize, CancellationToken cancellationToken)
        {
            var shows = await this.showsContext
                                  .Shows
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .Include(x => x.Cast)
                                  .ThenInclude(x => x.CastMember)
                                  .ToListAsync(cancellationToken);

            return shows.Select(x =>
                new Show
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cast = x.Cast
                            .Select(y =>
                                 new CastMember
                                 {
                                     Id = y.CastMember.Id,
                                     Name = y.CastMember.Name,
                                     Birthday = y.CastMember.Birthday
                                 })
                            .ToList()
                            .AsReadOnly()
                });
        }

        public async Task SaveShow(Show show, CancellationToken cancellationToken)
        {
            var castMembers = show.Cast
                                  .Select(x =>
                                       new Model.CastMember
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           Birthday = x.Birthday
                                       })
                                  .ToList();

            await showsContext.CastMembers
                              .UpsertRange(castMembers)
                              .NoUpdate()
                              .RunAsync(cancellationToken);

            showsContext.Shows.Add(
                new Model.Show
                {
                    Id = show.Id,
                    Name = show.Name,
                    Cast = castMembers.Select(x =>
                                           new Model.ShowCastMember
                                           {
                                               CastMember = x,
                                               CastMemberId = x.Id,
                                               ShowId = show.Id
                                           })
                                      .ToList()
                });

            await showsContext.SaveChangesAsync(cancellationToken);
        }
    }
}