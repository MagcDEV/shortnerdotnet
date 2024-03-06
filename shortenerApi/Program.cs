using Microsoft.EntityFrameworkCore;
using shortenerApi.Data;
using shortenerApi.Entities;
using shortenerApi.Models;
using shortenerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ShortUrlStore");
builder.Services.AddSqlite<AppDbContext>(connectionString);
builder.Services.AddScoped<UrlShortenerService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/shorten", async (ShortenUrlRequest request,
                            UrlShortenerService urlShortenerService,
                            AppDbContext appDbContext,
                            HttpContext httpContext) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("The specified URL is invalid.");
    }

    var code = await urlShortenerService.GenerateUniqueCode();

    var shortenedUrl = new ShortenedUrl{
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        Code = code,
        CreatedOnUtc = DateTime.UtcNow
    };

    await appDbContext.AddAsync(shortenedUrl);
    await appDbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);

});

app.MapGet("api/{code}", async (string code, AppDbContext appDbContext) => {
    var shortenedUrl = await appDbContext.ShortenedUrls.FirstOrDefaultAsync(x => x.Code == code);

    if (shortenedUrl == null)
    {
        return Results.NotFound();
    }

    return Results.Redirect(shortenedUrl.LongUrl);

});

await app.MigrateAsync();

app.Run();