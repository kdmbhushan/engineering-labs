# LINQ Internals: Iterator State Machines & Deferred Execution

> **Laboratory Experiment**: Dissecting C# LINQ under the hood: compiler-generated `IEnumerator<T>` state machines, closure capture allocations, and deferred execution pipelines.

🔗 **Canonical Deep Dive**: [LINQ in C# .NET: Under the Hood, State Machines & Allocation Profiling](https://bhushankadam.dev/blog/linq-in-c-sharp-part-2) on [bhushankadam.dev](https://bhushankadam.dev).

## Key Concepts
1. **Deferred Execution**: Queries do not materialize in memory until enumerated via `foreach`, `.ToList()`, or terminal aggregations.
2. **Iterator State Machines**: C# compiler emits a private hidden state machine class implementing `IEnumerator<T>` for methods using `yield return`.
3. **Closure Allocations**: Capturing external local variables into lambda expressions triggers compiler-generated display classes on the managed heap.
