using AcademyGestionGeneral;
using NLog;
using NLog.Web;

//var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
//var logger = LogManager.GetCurrentClassLogger();
//logger.Debug("init main");

var logger = NLog.LogManager.Setup()
    .LoadConfigurationFromFile()
    .GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    var startup = new Startup(builder.Configuration);

    startup.ConfigureServices(builder.Services);

    builder.Logging.ClearProviders();

    builder.Host.UseNLog();

    var app = builder.Build();

    startup.Configure(app, app.Environment);

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "El programa se detuvo por una falla.");
}
finally
{
    NLog.LogManager.Shutdown();
}