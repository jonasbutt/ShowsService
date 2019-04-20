using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShowsService.Data.Model;

namespace ShowsService.Data.Redis
{
    public class ShowsRepository : IShowsRepository
    {
        public Task<IEnumerable<Show>> GetShows(int pageNumber, short pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SaveShow(Show show, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}