using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KnowYourStack
{
    public class SloaderFunctions
    {
        [FunctionName("Sloader")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Sloader Function invoked at: {DateTime.Now}");

            log.LogInformation($"SloaderRunner Version: {typeof(Sloader.Engine.SloaderRunner).Assembly.GetName().Version}");

            Sloader.Config.SloaderConfigLocator locator = new Sloader.Config.SloaderConfigLocator();
            var allConfigs = locator.FromGitHub("Code-Inside", "KnowYourStack", "_data", "*.Sloader.yml").GetAwaiter().GetResult();

            foreach (var configUrl in allConfigs)
            {
                try
                {
                    log.LogInformation($"Sloader run for: {configUrl}");
                    Sloader.Engine.SloaderRunner.AutoRun(configUrl).GetAwaiter().GetResult();
                }
                catch(Exception exc)
                {
                    log.LogError("Exception: " + exc.Message);
                    log.LogError("Exception StackTrace: " + exc.StackTrace);
                }
            }

            log.LogInformation($"Sloader done at: {DateTime.Now}");

        }
    }

}
