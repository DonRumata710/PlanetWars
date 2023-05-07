using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Controllers
{
    public class LifetimeManager : IHostedService
    {
        public LifetimeManager(DatabaseService databaseService,
            IHostApplicationLifetime _appLifetime)
        {
            database = databaseService;
            appLifetime = _appLifetime;
        }
        private void OnStarted()
        {
            Log.Information("OnStarted has been called.");

            database.SetServerStatus(true);
        }

        private void OnStopped()
        {
            Log.Information("OnStopped has been called.");

            database.SetServerStatus(false);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask; 
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private DatabaseService database;
        private readonly IHostApplicationLifetime appLifetime;
    }
}
