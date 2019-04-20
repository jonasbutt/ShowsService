using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShowsService.Data.Model;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ShowsService.Data.Redis
{
    public class ShowsRepository : IShowsRepository
    {
        private const string ShowsKey = "shows";

        private readonly IRedisCacheClient redisCacheClient;

        public ShowsRepository(IRedisCacheClient redisCacheClient)
        {
            this.redisCacheClient = redisCacheClient;
        }

        public async Task<IEnumerable<Show>> GetShows(int pageNumber, short pageSize, CancellationToken cancellationToken)
        {
            return await this.redisCacheClient
                             .GetDbFromConfiguration()
                             .SortedSetRangeByScoreAsync<Show>(
                                  ShowsKey, 
                                  skip: (pageNumber - 1) * pageSize, 
                                  take: pageSize);
        }

        public async Task SaveShow(Show show, CancellationToken cancellationToken)
        {
            await this.redisCacheClient
                      .GetDbFromConfiguration()
                      .SortedSetAddAsync(
                           ShowsKey,
                           show,
                           show.Id);
        }
    }
}