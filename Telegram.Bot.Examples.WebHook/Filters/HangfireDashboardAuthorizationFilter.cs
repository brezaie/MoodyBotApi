using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Telegram.Bot.Filters;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }


    public void OnAuthorization(AuthorizationFilterContext context)
    {

    }

    public async Task<bool> AuthorizeAsync(DashboardContext context)
    {
        return await Task.Run(() => true);
    }
}
