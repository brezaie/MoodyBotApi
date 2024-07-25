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
        private readonly IJob _satisfactionReminderJob;
        private readonly IJob _reportJob;

        public JobController(IEnumerable<IJob> jobs)
        {
            _satisfactionReminderJob = jobs.FirstOrDefault(x => x.GetType() == typeof(SatisfactionReminderJob));
            _reportJob = jobs.FirstOrDefault(x => x.GetType() == typeof(ReportJob));
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

                RecurringJob.AddOrUpdate("SatisfactionReminderJob", () => _satisfactionReminderJob.Run(),
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
        [Route("CreateReportJob")]
        public ActionResult CreateReportJob(string cronExp)
        {
            try
            {
                RecurringJobOptions jobOptions = new()
                {
                    TimeZone = TimeZoneInfo.Utc
                };

                RecurringJob.AddOrUpdate("ReportJob", () => _reportJob.Run(),
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
