using System;
using System.Reactive.Concurrency;

namespace MusicStore.Utils;

public interface ISchedulerProvider
{
    TimeSpan CreateTime(TimeSpan time);

    IScheduler Scheduler { get; }
}
