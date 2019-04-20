using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using ShowsService.Data.Model;
using ShowsService.Ingester.TvMaze;

namespace ShowsService.Ingester.Jobs
{
    public interface IDownloadCastJob
    {
        [JobDisplayName("DOWNLOAD CAST: {0} {1}")]
        Task Run(long showId, string showName, IJobCancellationToken jobCancellationToken);
    }

    public class DownloadCastJob : IDownloadCastJob
    {
        private readonly ITvMazeClient tvMazeClient;

        public DownloadCastJob(ITvMazeClient tvMazeClient)
        {
            this.tvMazeClient = tvMazeClient;
        }

        public async Task Run(long showId, string showName, IJobCancellationToken jobCancellationToken)
        {
            var show = new Show
            {
                Id = showId,
                Name = showName,
                Cast = await this.GetCast(showId, jobCancellationToken.ShutdownToken)
            };

            BackgroundJob.Enqueue<ISaveShowJob>(
                job => job.Run(show, JobCancellationToken.Null));
        }

        public async Task<IReadOnlyCollection<CastMember>> GetCast(long showId, CancellationToken cancellationToken)
        {
            var tvMazePersons = await this.tvMazeClient.GetCast(showId, cancellationToken);
            return tvMazePersons?.GroupBy(x => x.Id)
                                 .Select(x => x.First())
                                 .Select(x =>
                                      new CastMember
                                      {
                                          Id = x.Id,
                                          Name = x.Name,
                                          Birthday = x.Birthday
                                      })
                                 .ToList();
        }
    }
}
