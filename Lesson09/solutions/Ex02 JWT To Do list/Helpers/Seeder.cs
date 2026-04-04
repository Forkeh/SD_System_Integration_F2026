using Ex_07_To_Do_List.Data;

namespace Ex_07_To_Do_List.Helpers;

public class Seeder
{
    public static void SeedUsers(AppDbContext db)
    {
        if (db.Users.Any())
        {
            return;
        }

        db.Users.Add(new User
        {
            Username = "brian",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("secret123")
        });
        db.SaveChanges();
    }
}