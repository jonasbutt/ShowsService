using Hangfire.Dashboard;

namespace ShowsService.Ingester.Hangfire
{
    public class PublicAccessDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}