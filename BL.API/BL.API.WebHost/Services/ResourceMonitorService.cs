using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.WebHost.Services
{
    public class ResourceMonitorService : IHostedService, IDisposable
    {
        private bool disposedValue;
        private readonly ILogger<ResourceMonitorService> _logger;
        private Timer _timer;
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;

        public ResourceMonitorService(ILogger<ResourceMonitorService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Resource monitor is starting.");

            //InitializeCPUCounter();
            //InitializeRAMCounter();
            _timer = new Timer(LogResources, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));

            return Task.CompletedTask;
        }

        private void LogResources(object? state)
        {
            using (var proc = Process.GetCurrentProcess())
            {
                //var msg = $"CPUI: ${_cpuCounter.NextValue()}; RAM: {_ramCounter.NextValue()}; PrivateMemory: {proc.PrivateMemorySize64 / (1024 * 1024)}";
                var msg = $"PrivateMemory: {proc.PrivateMemorySize64 / (1024 * 1024)}";
                var additionalInfo = 

                $"  Physical memory usage     : {proc.WorkingSet64 / 1024}MB \n"
                + $"  Base priority             : {proc.BasePriority}\n"
                + $"  Priority class            : {proc.PriorityClass}\n"
                + $"  User processor time       : {proc.UserProcessorTime}\n"
                + $"  Privileged processor time : {proc.PrivilegedProcessorTime}\n"
                + $"  Total processor time      : {proc.TotalProcessorTime}\n"
                + $"  Paged system memory size  : {proc.PagedSystemMemorySize64}\n"
                + $"  Paged memory size         : {proc.PagedMemorySize64}\n";

                _logger.LogInformation(msg);
                _logger.LogInformation(additionalInfo);
            }
        }

        private void InitializeCPUCounter()
        {
            if (OperatingSystem.IsWindows())
            {
                _cpuCounter = new PerformanceCounter(
            "Processor",
            "% Processor Time",
            "_Total",
            true
            );
            }
        }

        private void InitializeRAMCounter()
        {
            if (OperatingSystem.IsWindows())
            {
                _ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Resource monitor is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
