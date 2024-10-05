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
        private readonly IJob _emotionReminderJob;
        private readonly IJob _adminStatisticsJob;

        public JobController(IEnumerable<IJob> jobs, IJob emotionReminderJob)
        {
            _satisfactionReminderJob = jobs.FirstOrDefault(x => x.GetType() == typeof(SatisfactionReminderJob));
            _reportJob = jobs.FirstOrDefault(x => x.GetType() == typeof(ReportJob));
            _emotionReminderJob = jobs.FirstOrDefault(x => x.GetType() == typeof(EmotionReminderJob));
            _adminStatisticsJob = jobs.FirstOrDefault(x => x.GetType() == typeof(AdminStatisticsJob));
        }

        [HttpGet]
        [Route("CreateAdminStatisticsJob")]
        public ActionResult CreateAdminStatisticsJob(string cronExp)
        {
            try
            {
                RecurringJobOptions jobOptions = new()
                {
                    TimeZone = TimeZoneInfo.Utc
                };

                RecurringJob.AddOrUpdate("AdminStatisticsJob", () => _adminStatisticsJob.Run(),
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
        [Route("CreateEmotionReminderJob")]
        public ActionResult CreateEmotionReminderJob(string cronExp)
        {
            try
            {
                RecurringJobOptions jobOptions = new()
                {
                    TimeZone = TimeZoneInfo.Utc
                };

                RecurringJob.AddOrUpdate("EmotionReminderJob", () => _emotionReminderJob.Run(),
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
