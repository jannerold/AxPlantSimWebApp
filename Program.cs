using AxPlantSimWebApp.Configuration;
using AxPlantSimWebApp.Data;
using AxPlantSimWebApp.Services;
using AxPlantSimWebApp.Simulation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// MVC
// -----------------------------
builder.Services.AddControllersWithViews();

// -----------------------------
// Konfiguraèní soubory
// -----------------------------
builder.Configuration.AddJsonFile(
  "columnNames.json",
  optional: true,
  reloadOnChange: true
);

// -----------------------------
// EF Core + SQLite
// -----------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlite(
    builder.Configuration.GetConnectionString("SQLite")
  )
);

// -----------------------------
// Application services
// -----------------------------
builder.Services.AddScoped<TableBrowserService>();
builder.Services.AddScoped<SimulationService>();
builder.Services.AddScoped<SimulationConfigService>();

builder.Services.AddSingleton<ColumnNameMapper>();

// -----------------------------
// External DB configuration
// -----------------------------
builder.Services.Configure<ExternalDbOptions>(
  builder.Configuration.GetSection("ExternalDb")
);

// -----------------------------
// Simulation agent client
// -----------------------------
builder.Services.AddHttpClient<ISimulationExecutor, AgentSimulationExecutor>(client =>
{
  client.BaseAddress = new Uri("http://localhost:5005/");
  client.Timeout = TimeSpan.FromMinutes(5);
});

var app = builder.Build();

// -----------------------------
// Middleware pipeline
// -----------------------------
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// -----------------------------
// Routing
// -----------------------------
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}"
);

// -----------------------------
// DB bootstrap (NE EF migrace)
// -----------------------------
DatabaseInitializer.EnsureImportRunTable(
  builder.Configuration.GetConnectionString("SQLite")!
);

app.Run();
