using System.ComponentModel.DataAnnotations.Schema;

namespace Ex_07_To_Do_List.Data;

public class User
{
    public int Id { get; set; }

    [Column("username")] public string Username { get; set; } = string.Empty;

    [Column("password_hash")] public string PasswordHash { get; set; } = string.Empty;
}