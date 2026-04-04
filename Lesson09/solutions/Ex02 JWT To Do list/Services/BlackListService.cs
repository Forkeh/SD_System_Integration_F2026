using StackExchange.Redis;

namespace Ex_07_To_Do_List.Services;

public class BlacklistService(IConnectionMultiplexer redis)
{
    private readonly IDatabase _db = redis.GetDatabase();

    // Store JTI with a TTL so it auto-expires when the token would have expired anyway
    public Task BlacklistAsync(string jti, TimeSpan ttl)
    {
        return _db.StringSetAsync($"blacklist:{jti}", "1", ttl);
    }

    public async Task<bool> IsBlacklistedAsync(string jti)
    {
        return await _db.KeyExistsAsync($"blacklist:{jti}");
    }
}