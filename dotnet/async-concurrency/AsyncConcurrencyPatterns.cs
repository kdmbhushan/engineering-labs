using System.Threading.Channels;

namespace EngineeringLabs.AsyncConcurrency;

public sealed class ValueTaskCache
{
    private readonly Dictionary<string, string> _memoryStore = new();

    public void Set(string key, string value) => _memoryStore[key] = value;

    // Returns ValueTask to eliminate Task heap allocation on cache hits
    public ValueTask<string?> GetAsync(string key)
    {
        if (_memoryStore.TryGetValue(key, out var cached))
        {
            return ValueTask.FromResult<string?>(cached);
        }

        return new ValueTask<string?>(FetchFromRemoteAsync(key));
    }

    private async Task<string?> FetchFromRemoteAsync(string key)
    {
        await Task.Delay(10); // Simulated network I/O
        return null;
    }
}

public sealed class ProducerConsumerChannel<T>
{
    private readonly Channel<T> _channel;

    public ProducerConsumerChannel(int capacity = 100)
    {
        _channel = Channel.CreateBounded<T>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleWriter = false,
            SingleReader = false
        });
    }

    public async ValueTask PublishAsync(T item, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(item, ct);
    }

    public void Complete() => _channel.Writer.Complete();

    public IAsyncEnumerable<T> ReadAllAsync(CancellationToken ct = default)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}
