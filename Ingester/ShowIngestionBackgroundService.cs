using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using ShowsService.Data;
using ShowsService.Ingester.Jobs;

namespace ShowsService.Ingester
{
    public class ShowIngestionBackgroundService : BackgroundService
    {
        private const int FirstPageNumber = 0;

        private readonly IShowsRepository showsRepository;

        public ShowIngestionBackgroundService(IShowsRepository showsRepository)
        {
            this.showsRepository = showsRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var shows = await this.showsRepository.GetShows(1, 1, cancellationToken);

            var showIngestionHasStarted = shows.Any();
            if (showIngestionHasStarted)
            {
                return;
            }

            BackgroundJob.Enqueue<IDownloadShowsJob>(
                job => job.Run(FirstPageNumber, JobCancellationToken.Null));
        }
    }
}
