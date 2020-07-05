using System.Threading;
using System.Threading.Tasks;
using ImageUpload.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ImageUpload.HealthCheck
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IImageUploaderStorageService _storageService;

        public StorageHealthCheck(IImageUploaderStorageService storageService)
        {
            _storageService = storageService;
        }


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var isStorageOk = await _storageService.CheckHealthAsync();
            return new HealthCheckResult(isStorageOk ? HealthStatus.Healthy : HealthStatus.Unhealthy);
        }
    }
}