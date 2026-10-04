using System.Text.Json;
using System.Text.Json.Serialization;

namespace EngineeringLabs.JsonSerialization;

public sealed record UserProfile(
    int Id,
    string Username,
    string Email,
    IReadOnlyList<string> Roles,
    DateTime CreatedAtUtc
);

[JsonSerializable(typeof(UserProfile))]
[JsonSerializable(typeof(IReadOnlyList<UserProfile>))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}

public static class AotJsonHelper
{
    public static string SerializeProfile(UserProfile profile)
    {
        return JsonSerializer.Serialize(profile, AppJsonSerializerContext.Default.UserProfile);
    }

    public static UserProfile? DeserializeProfile(string json)
    {
        return JsonSerializer.Deserialize(json, AppJsonSerializerContext.Default.UserProfile);
    }
}
