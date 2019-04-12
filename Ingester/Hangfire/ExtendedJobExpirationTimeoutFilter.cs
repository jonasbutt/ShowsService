using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace ShowsService.Ingester.Hangfire
{
    public class ExtendedJobExpirationTimeoutFilter : JobFilterAttribute, IApplyStateFilter
    {
        private static readonly TimeSpan JobExpirationTimeout = TimeSpan.FromDays(100);

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = JobExpirationTimeout;
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}