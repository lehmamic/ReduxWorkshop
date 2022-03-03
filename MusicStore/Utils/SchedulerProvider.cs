using System;
using System.Reactive.Concurrency;

namespace MusicStore.Utils;

public class SchedulerProvider : ISchedulerProvider
{
    public SchedulerProvider(IScheduler scheduler)
    {
        Scheduler = scheduler;
    }

    public IScheduler Scheduler { get; }

    public TimeSpan CreateTime(TimeSpan time)
    {
        return time;
    }
}
