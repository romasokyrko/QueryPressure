﻿using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles
{
    //public class LimitedConcurrencyLoadProfilу: IProfile
    //{
    //    private readonly IProfile _internal;
    //    private readonly TimeSpan _delay;
    //    private readonly ConcurrentDictionary<Guid, DateTime> _dateTimes = new();

    //    public LimitedConcurrencyLoadProfile(int limit, TimeSpan delay)
    //    {
    //        _delay = delay;
    //        _internal = new LimitedConcurrencyLoadProfile(limit);
    //        _delays = new List<Task>(limit);
    //    }


    //    public async Task<bool> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    //    {
    //        await _internal.WhenNextCanBeExecutedAsync(cancellationToken);
    //        bool shouldWait = _semaphore.CurrentCount >= _limit;
    //        _delays.Remove(await Task.WhenAny(_delays));

    //    }
    //}


    public class LimitedConcurrencyLoadProfile : IProfile
    {
        private readonly int _limit;
        private readonly SemaphoreSlim _semaphore;

        public LimitedConcurrencyLoadProfile(int limit)
        {
            _limit = limit;
            _semaphore = new SemaphoreSlim(limit);
        }

        public async Task<bool> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
        {


            await _semaphore.WaitAsync(cancellationToken);



            return !cancellationToken.IsCancellationRequested;
        }

        public Task OnQueryExecutedAsync(CancellationToken cancellationToken)
        {
            _semaphore.Release();
            return Task.CompletedTask;
        }

    }
}
