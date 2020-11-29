using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.UI.HealthChecks
{
    public class HomeHealthCheck : IHealthCheck
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeHealthCheck(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext == null)
                return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
            var request = _httpContextAccessor.HttpContext.Request;
            var myUrl = request.Scheme + "://" + request.Host;

            var client = new HttpClient();
            var response = await client.GetAsync(myUrl, cancellationToken);
            var pageContents = await response.Content.ReadAsStringAsync(cancellationToken);
            return pageContents.Contains("product") ? HealthCheckResult.Healthy("The check indicates a healthy result.") : HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
        }
    }
}
