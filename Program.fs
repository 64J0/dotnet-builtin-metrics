open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe

let SUCCESS_CODE = 0
let FAILURE_CODE = 1

let webApp =
    choose
        [ route "/ping" >=> setStatusCode 200 >=> Handlers.Ping.GET.ping
          //   route "/healthz" >=> setStatusCode 200 >=> Healthz.GET.getHealthz
          ]

let configureApp (app: IApplicationBuilder) = app.UseGiraffe webApp

let configureServices (services: IServiceCollection) =
    services.AddGiraffe().AddMetrics().AddSingleton<Metrics.PingMetrics>()
    // .AddResourceMonitoring()
    // .AddHealthChecks()
    // .AddResourceUtilizationHealthCheck()
    |> ignore

[<EntryPoint>]
let main (_args: string array) : int =
    try
        Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    .UseUrls([| "http://localhost:3000" |])
                |> ignore)
            .Build()
            .Run()

        SUCCESS_CODE
    with exn ->
        [ "Server did not start successfully"; $"{exn.Message}"; $"{exn.StackTrace}" ]
        |> List.iter (eprintfn "%s")

        FAILURE_CODE
