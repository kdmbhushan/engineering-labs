# Memory Caching: High-Concurrency Stampede Mitigation

> **Laboratory Experiment**: Mitigation of cache stampedes and dogpiling under high concurrent load using SemaphoreSlim locks and .NET 9 HybridCache design patterns.

🔗 **Canonical Deep Dive**: [In-Memory Caching in .NET Core: Step by Step](https://bhushankadam.dev/blog/memory-caching-in-net-core-step-by-step) on [bhushankadam.dev](https://bhushankadam.dev).

## Problem Statement
When high-traffic cache entries expire, hundreds of concurrent threads can simultaneously encounter a cache miss, causing all of them to query the database or downstream service simultaneously. This "thundering herd" causes CPU spikes, connection pool exhaustion, and cascading database failure.

## Solution Architecture
`StampedeSafeMemoryCache` implements a per-key double-checked locking mechanism using `ConcurrentDictionary<string, SemaphoreSlim>`:
1. Thread-safe fast path checks cache with zero locking.
2. On cache miss, threads acquire a lock isolated to that specific cache key.
3. First thread loads data, populates cache, and releases lock.
4. Subsequent waiting threads read the newly populated cache entry without hitting backend storage.
