namespace Handlers.Healthz

open Microsoft.Extensions.Diagnostics.HealthChecks
open Microsoft.AspNetCore.Http
open Giraffe

// https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostic-health-checks?view=aspnetcore-8.0

module GET =

    let getHealthz: HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let healthCheckService = ctx.GetService<HealthCheckService>()

                let! result = healthCheckService.CheckHealthAsync()
                let resultMsg = $"{result.Status} {result.TotalDuration}"

                return! text resultMsg next ctx
            }
