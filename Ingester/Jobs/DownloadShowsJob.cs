using System;
using System.Threading.Tasks;
using Hangfire;
using ShowsService.Ingester.TvMaze;

namespace ShowsService.Ingester.Jobs
{
    public interface IDownloadShowsJob
    {
        [JobDisplayName("DOWNLOAD SHOWS: {0}")]
        Task Run(int pageNumber, IJobCancellationToken jobCancellationToken);
    }

    public class DownloadShowsJob : IDownloadShowsJob
    {
        // According to the documentation the /shows resource is cached for 24 hours,
        // so our ingestion logic uses the same interval.
        private static readonly TimeSpan ShowUpdateInterval = TimeSpan.FromDays(1); 

        private readonly ITvMazeClient tvMazeClient;

        public DownloadShowsJob(ITvMazeClient tvMazeClient)
        {
            this.tvMazeClient = tvMazeClient;
        }

        public async Task Run(int pageNumber, IJobCancellationToken jobCancellationToken)
        {
            var tvMazeShows = await this.tvMazeClient.GetShows(pageNumber, jobCancellationToken.ShutdownToken);

            var noMoreTvMazeShowsAvailable = tvMazeShows == null;
            if (noMoreTvMazeShowsAvailable)
            {
                var nextTimeToCheck = DateTimeOffset.Now.Add(ShowUpdateInterval);
                BackgroundJob.Schedule<IDownloadShowsJob>(
                    job => job.Run(pageNumber, JobCancellationToken.Null), nextTimeToCheck);
                return;
            }

            var nextPageNumber = pageNumber + 1;
            BackgroundJob.Enqueue<IDownloadShowsJob>(
                job => job.Run(nextPageNumber, JobCancellationToken.Null));

            foreach (var tvMazeShow in tvMazeShows)
            {
                BackgroundJob.Enqueue<IDownloadCastJob>(
                    job => job.Run(tvMazeShow.Id, tvMazeShow.Name, JobCancellationToken.Null));
            }
        }
    }
}
