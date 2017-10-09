#define TRACE

using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

public static void Run(TimerInfo everyDay, TraceWriter log)
{ 
    log.Info($"Sloader run.csx invoked at: {DateTime.Now}");    

    var listener = new SystemTraceListener(log);

    log.Info($"SloaderRunner Version: {typeof(Sloader.Engine.SloaderRunner).Assembly.GetName().Version}");       

    Sloader.Config.SloaderConfigLocator locator = new Sloader.Config.SloaderConfigLocator();
    var allConfigs = locator.FromGitHub("Code-Inside", "KnowYourStack", "_data", "*.Sloader.yml").GetAwaiter().GetResult();

    foreach(var configUrl in allConfigs) {
        log.Info($"Sloader run for: {configUrl}");    
        Sloader.Engine.SloaderRunner.AutoRun(configUrl).GetAwaiter().GetResult();
    }  

    log.Info($"Sloader run.csx done at: {DateTime.Now}");    

}

public class SystemTraceListener : TraceListener
{
    private TraceWriter _log;

    public SystemTraceListener(TraceWriter log)
    {
        Trace.Listeners.Add(this);
        this._log = log;
        _log.Info("SystemTraceListener is initialized");
    }

    public override void Write(string message)
    {
        _log.Info("Trace: " + message);
    }

    public override void WriteLine(string message)
    {
        _log.Info("Trace: " + message);
    }
}