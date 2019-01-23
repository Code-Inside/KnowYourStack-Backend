using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KnowYourStack
{
    public static class SloaderFunctions
    {
        [FunctionName("Sloader.TimeTrigger")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            log.LogInformation($"Sloader Function invoked at: {DateTime.Now}");

            log.LogInformation($"SloaderRunner Version: {typeof(Sloader.Engine.SloaderRunner).Assembly.GetName().Version}");

            Sloader.Config.SloaderConfigLocator locator = new Sloader.Config.SloaderConfigLocator();
            var allConfigs = locator.FromGitHub("Code-Inside", "KnowYourStack", "_data", "*.Sloader.yml").GetAwaiter().GetResult();

            foreach (var configUrl in allConfigs)
            {
                log.LogInformation($"Sloader run for: {configUrl}");
                Sloader.Engine.SloaderRunner.AutoRun(configUrl).GetAwaiter().GetResult();
            }

            log.LogInformation($"Sloader done at: {DateTime.Now}");

        }
    }
}
