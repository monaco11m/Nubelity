

using Microsoft.EntityFrameworkCore;
using Nubelity.Domain.Entities;

namespace Nubelity.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task InitializeAsync(LibraryDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    PasswordHash = "admin123",
                    Role = "Admin"
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
