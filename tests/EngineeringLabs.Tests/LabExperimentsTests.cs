using Configuration;
using EngineeringLabs.AsyncConcurrency;
using EngineeringLabs.JsonSerialization;
using EngineeringLabs.LinqInternals;
using EngineeringLabs.MemoryCaching;
using JwtAuthentication.Models;
using Microsoft.Extensions.Caching.Memory;
using SendingEmail.Configuration;
using SendingEmail.Models;
using Xunit;

namespace EngineeringLabs.Tests;

public class LabExperimentsTests
{
    [Fact]
    public void OptionsPattern_ShouldBindCompanySettingsProperties()
    {
        var settings = new CompanySettings
        {
            Name = "Contoso Engineering",
            Email = "contact@contoso.com",
            Contact = "+1-555-0100"
        };

        Assert.Equal("Contoso Engineering", settings.Name);
        Assert.Equal("contact@contoso.com", settings.Email);
        Assert.Equal("+1-555-0100", settings.Contact);
    }

    [Fact]
    public void JwtAuthentication_ShouldValidateUserRegistrationModel()
    {
        var reg = new UserRegistration
        {
            Username = "bhushankadam",
            Email = "bhushan@example.com",
            Password = "P@ssword123!"
        };

        Assert.Equal("bhushankadam", reg.Username);
        Assert.Equal("bhushan@example.com", reg.Email);
        Assert.Equal("P@ssword123!", reg.Password);
    }

    [Fact]
    public void TransactionalEmail_ShouldConfigureSmtpSettingsAndEmailViewModel()
    {
        var smtp = new SMTPSettings
        {
            Host = "smtp.sendgrid.net",
            Port = 587,
            FromName = "Bhushan Kadam",
            FromEmail = "noreply@bhushankadam.dev",
            Username = "apikey",
            Password = "fake-password"
        };

        var emailVm = new EmailViewModel
        {
            To = "user@example.com",
            Subject = "Test Subject",
            Body = "Test Body Content"
        };

        Assert.Equal(587, smtp.Port);
        Assert.Equal("user@example.com", emailVm.To);
        Assert.Equal("Test Subject", emailVm.Subject);
    }

    [Fact]
    public async Task MemoryCaching_StampedeSafeCache_ShouldPreventStampede()
    {
        using var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var safeCache = new StampedeSafeMemoryCache(memoryCache);
        var fetchCount = 0;

        async Task<string> Fetch()
        {
            Interlocked.Increment(ref fetchCount);
            await Task.Delay(50);
            return "cached_payload";
        }

        // Spawn 10 concurrent requests for the exact same key
        var tasks = Enumerable.Range(0, 10).Select(_ =>
            safeCache.GetOrCreateAsync("high_traffic_key", _ => Fetch(), TimeSpan.FromMinutes(5))
        ).ToList();

        var results = await Task.WhenAll(tasks);

        Assert.All(results, res => Assert.Equal("cached_payload", res));
        Assert.Equal(1, fetchCount); // Only 1 backend call made despite 10 concurrent threads!
    }

    [Fact]
    public async Task AsyncConcurrency_ValueTaskCache_ShouldReturnCachedValueWithoutAllocation()
    {
        var cache = new ValueTaskCache();
        cache.Set("hero_key", "hero_value");

        var vt = cache.GetAsync("hero_key");
        Assert.True(vt.IsCompletedSuccessfully); // Synchronous completion!

        var val = await vt;
        Assert.Equal("hero_value", val);
    }

    [Fact]
    public async Task AsyncConcurrency_ProducerConsumerChannel_ShouldStreamItems()
    {
        var channel = new ProducerConsumerChannel<int>(capacity: 10);

        await channel.PublishAsync(101);
        await channel.PublishAsync(102);
        channel.Complete();

        var received = new List<int>();
        await foreach (var item in channel.ReadAllAsync())
        {
            received.Add(item);
        }

        Assert.Equal(new[] { 101, 102 }, received);
    }

    [Fact]
    public void JsonSerialization_SourceGenerated_ShouldSerializeAndDeserialize()
    {
        var profile = new UserProfile(
            Id: 42,
            Username: "kdmbhushan",
            Email: "bhushan@example.com",
            Roles: new[] { "Admin", "Engineer" },
            CreatedAtUtc: new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        );

        var json = AotJsonHelper.SerializeProfile(profile);
        Assert.Contains("\"Username\":\"kdmbhushan\"", json);

        var restored = AotJsonHelper.DeserializeProfile(json);
        Assert.NotNull(restored);
        Assert.Equal(42, restored.Id);
        Assert.Equal("kdmbhushan", restored.Username);
        Assert.Equal(2, restored.Roles.Count);
    }

    [Fact]
    public void LinqInternals_IteratorStateMachine_ShouldDeferExecution()
    {
        var steppedCount = 0;
        var query = LinqInternalsDemo.GenerateStreamingNumbers(5, onStep: _ => steppedCount++);

        Assert.Equal(0, steppedCount); // Deferred! Has not executed yet.

        var list = query.ToList(); // Materialize
        Assert.Equal(5, steppedCount);
        Assert.Equal(new[] { 2, 4, 6, 8, 10 }, list);
    }

    [Fact]
    public void DockerContainerApps_WeatherForecastRecord_ShouldComputeFahrenheit()
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.UtcNow), 20, "Mild");
        Assert.Equal(20, forecast.TemperatureC);
        Assert.Equal(67, forecast.TemperatureF);
    }
}
