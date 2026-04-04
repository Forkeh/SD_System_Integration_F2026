using Microsoft.EntityFrameworkCore;

namespace Ex_07_To_Do_List.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}