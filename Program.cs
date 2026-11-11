using Playground.Components;
using Playground.Data;
using Playground.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

var connectionString = builder.Configuration.GetConnectionString("WarehouseDb")
    ?? throw new InvalidOperationException("Connection string 'WarehouseDb' was not found.");

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"))
    .SetApplicationName("Playground");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<DirectoryService>();
builder.Services.AddScoped<ProductService>();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbFactory = scope.ServiceProvider
        .GetRequiredService<IDbContextFactory<ApplicationDbContext>>();

    await using var db = await dbFactory.CreateDbContextAsync();
    await DatabaseSeeder.SeedAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
