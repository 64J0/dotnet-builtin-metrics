namespace Metrics

open System
open System.Diagnostics.Metrics

type TrackedRequestDuration(histogram: Histogram<float>) =
    let _requestStartTime = TimeProvider.System.GetTimestamp()
    let _histogram = histogram

    interface IDisposable with
        member _.Dispose() =
            let elapsed = TimeProvider.System.GetElapsedTime(_requestStartTime)
            _histogram.Record(elapsed.TotalMilliseconds)

type PingMetrics(meterFactory: IMeterFactory) =
    let meter: Meter = meterFactory.Create("MyApi.Ping")

    let _requestCounter: Counter<int> =
        meter.CreateCounter<int>("api.ping_requests.count")

    let _requestDuration: Histogram<float> =
        meter.CreateHistogram<float>("api.ping_requests.duration", "ms")

    member _.IncreaseRequestCounter() : unit = _requestCounter.Add(1)

    member _.TrackRequestDuration() : TrackedRequestDuration =
        new TrackedRequestDuration(_requestDuration)
