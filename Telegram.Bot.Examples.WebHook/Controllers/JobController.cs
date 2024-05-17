using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Examples.WebHook.Jobs;

namespace Telegram.Bot.Examples.WebHook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJob _job;

        public JobController(IJob job)
        {
            _job = job;
        }

        [HttpGet]
        [Route("CreateSatisfactionReminderJob")]
        public ActionResult CreateSatisfactionReminderJob()
        {
            RecurringJobOptions jobOptions = new()
            {
                TimeZone = TimeZoneInfo.Utc
            };

            RecurringJob.AddOrUpdate("SatisfactionReminderJob", () => _job.Run(),
                "* * * * *", jobOptions);

            return Ok();
        }

        [HttpGet]
        [Route("RemoveSatisfactionReminderJob")]
        public ActionResult RemoveSatisfactionReminderJob()
        {
            RecurringJob.RemoveIfExists("SatisfactionReminderJob");

            return Ok();
        }
    }
}
