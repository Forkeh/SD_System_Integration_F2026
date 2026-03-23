using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using StackExchange.Redis;

namespace Ex_07_To_Do_List.Helpers;

public class Helpers(SqliteConnection sqliteConn, IDatabase redis)
{
    public string GenerateToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }

    public bool VerifyUser(string username, string password)
    {
        var cmd = new SqliteCommand(
            "SELECT password_hash FROM users WHERE username = @u", sqliteConn);
        cmd.Parameters.AddWithValue("@u", username);
        var hash = cmd.ExecuteScalar() as string;
        return hash != null && BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public string? GetUsernameFromToken(string token)
    {
        return redis.StringGet($"token:{token}");
    }
}