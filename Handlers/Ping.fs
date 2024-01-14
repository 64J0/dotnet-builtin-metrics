namespace Handlers.Ping

open Microsoft.AspNetCore.Http
open Giraffe

// https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostic-health-checks?view=aspnetcore-8.0

module GET =

    let ping: HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let pingMetrics: Metrics.PingMetrics = ctx.GetService<Metrics.PingMetrics>()

            use _ = pingMetrics.TrackRequestDuration()

            pingMetrics.IncreaseRequestCounter()

            text "pong" next ctx
