# JSON Serialization: Source Generators & Native AOT

> **Laboratory Experiment**: High-performance JSON serialization using C# System.Text.Json source generation, Native AOT trimming compatibility, and zero-reflection throughput.

🔗 **Canonical Deep Dive**: [JSON Serialization and Deserialization in C# .NET: Source Generators vs Reflection](https://bhushankadam.dev/blog/json-serialization-and-deserialization-in-net) on [bhushankadam.dev](https://bhushankadam.dev).

## Key Patterns Demonstrated
1. **Source Generation**: Precomputes serialization metadata at compile time, eliminating runtime reflection overhead and warmup penalties.
2. **Native AOT Compatible**: Compatible with trimming and ahead-of-time compilation for instant startup and low memory footprint.
3. **Type Safety**: Generates strongly typed contract metadata via `JsonSerializerContext`.
