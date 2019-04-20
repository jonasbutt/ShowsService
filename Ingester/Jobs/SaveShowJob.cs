using System.Threading.Tasks;
using Hangfire;
using ShowsService.Data;
using ShowsService.Data.Model;

namespace ShowsService.Ingester.Jobs
{
    public interface ISaveShowJob
    {
        [JobDisplayName("SAVE SHOW: {0}")]
        Task Run(Show show, IJobCancellationToken jobCancellationToken);
    }

    public class SaveShowJob : ISaveShowJob
    {
        private readonly IShowsRepository showsRepository;

        public SaveShowJob(IShowsRepository showsRepository)
        {
            this.showsRepository = showsRepository;
        }

        public async Task Run(Show show, IJobCancellationToken jobCancellationToken)
        {
            await this.showsRepository.SaveShow(show, jobCancellationToken.ShutdownToken);
        }
    }
}