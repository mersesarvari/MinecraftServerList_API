using MSLServer.Logic;
using System.Net.NetworkInformation;
using System.Threading;


public class ServerStatusCheckerBackgroundWorker
    : BackgroundService
{
    private readonly PeriodicTimer timer;
    private readonly ILogger<ServerStatusCheckerBackgroundWorker> logger;
    private IServerRepository serverRepo;

    public ServerStatusCheckerBackgroundWorker(ILogger<ServerStatusCheckerBackgroundWorker> _logger, PeriodicTimer _timer, IServerRepository _serverRepo)
    {
        timer = _timer;
        logger = _logger;
        serverRepo = _serverRepo;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("BackgroundWorkerService started");
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("BackgroundWorkerService completed a task");
                await DoWorkAsync();

            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }
        
    }
    private async Task DoWorkAsync()
    {
        try
        {
            //Console.WriteLine(DateTime.Now.ToString("O"));
            serverRepo.CheckAllServerStatus();
            logger.LogInformation("[ServerStatusBW] : Server status check");
        }
        catch (Exception ex)
        {

            await Console.Out.WriteLineAsync(ex.Message);
        }
        

    }
}