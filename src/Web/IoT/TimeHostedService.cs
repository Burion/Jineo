using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jineo.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Jineo.Data 
{

    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        IServiceProvider sp;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceProvider sp)
        {
            _logger = logger;
            this.sp = sp;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            Random r = new Random();
            string sensorName = "";
            using (var scope = sp.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                foreach(var sensor in ctx.Sensors)
                {
                    var meterings = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MeteringModel>>(sensor.Data);
                    meterings.Add(new MeteringModel() { value = r.Next(50, 100), date = DateTime.Now });
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(meterings);
                    sensor.Data = json;
                    sensorName += sensor.Name;
                    sensorName += " ";
                }
                ctx.SaveChanges();
            }
            _logger.LogInformation(
                $"New metering was added for sensors: {sensorName}");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}