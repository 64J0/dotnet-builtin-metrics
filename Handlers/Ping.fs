namespace Handlers.Ping

open Microsoft.AspNetCore.Http
open Giraffe

module GET =

    let ping: HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let pingMetrics: Metrics.PingMetrics = ctx.GetService<Metrics.PingMetrics>()

            use _ = pingMetrics.TrackRequestDuration()

            pingMetrics.IncreaseRequestCounter()

            text "pong" next ctx
