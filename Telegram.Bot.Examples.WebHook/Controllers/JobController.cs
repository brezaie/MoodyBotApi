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
        public ActionResult CreateSatisfactionReminderJob(string cronExp)
        {
            try
            {
                RecurringJobOptions jobOptions = new()
                {
                    TimeZone = TimeZoneInfo.Utc
                };

                RecurringJob.AddOrUpdate("SatisfactionReminderJob", () => _job.Run(),
                    cronExp, jobOptions);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("RemoveSatisfactionReminderJob")]
        public ActionResult RemoveSatisfactionReminderJob()
        {
            RecurringJob.RemoveIfExists("SatisfactionReminderJob");

            return Ok();
        }

        [HttpGet]
        [Route("IsAlive")]
        public ActionResult IsAlive()
        {
            try
            {
                return Ok("Yes");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest($"No, because {ex.Message}");
            }
        }
    }
}
