# Async Concurrency: Thread Pool Starvation & ValueTask Optimization

> **Laboratory Experiment**: Deep dive into async/await mechanics, ThreadPool thread starvation prevention, non-blocking Channels, and ValueTask memory allocation efficiency.

🔗 **Canonical Deep Dive**: [Asynchronous Programming in C# .NET: Under the Hood](https://bhushankadam.dev/blog/asynchronous-programming-in-c-net) on [bhushankadam.dev](https://bhushankadam.dev).

## Key Patterns Demonstrated
1. **ValueTask Zero-Allocation**: Eliminates `Task<T>` object heap allocations when data is synchronously available (such as cache hits).
2. **Channel Producer-Consumer**: Replaces heavy locking collections with bounded `System.Threading.Channels` supporting backpressure and async streaming.
3. **Deadlock Prevention**: Mitigates sync-over-async thread starvation traps (`.Result`, `.Wait()`).
