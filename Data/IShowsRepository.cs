using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShowsService.Data.Model;

namespace ShowsService.Data
{
    public interface IShowsRepository
    {
        Task<IEnumerable<Show>> GetShows(int pageNumber, short pageSize, CancellationToken cancellationToken);

        Task SaveShow(Show show, CancellationToken cancellationToken);
    }
}