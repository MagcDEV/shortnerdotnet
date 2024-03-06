using Microsoft.EntityFrameworkCore;

namespace shortenerApi.Data;

public static class DataExtensions
{
    public static async Task MigrateAsync(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();

    }

}
