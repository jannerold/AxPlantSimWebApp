using AxPlantSimWebApp.Data;
using AxPlantSimWebApp.Services;
using AxPlantSimWebApp.Simulation;
using AxPlantSimWebApp.Configuration;
using AxPlantSimWebApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile(
  "columnNames.json",
  optional: true,
  reloadOnChange: true
);

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(
  options => options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"))
);

// application services
builder.Services.AddScoped<TableBrowserService>();
builder.Services.AddSingleton<ColumnNameMapper>();
builder.Services.AddScoped<SimulationService>();
builder.Services.AddScoped<SimulationConfigService>();
builder.Services.Configure<ExternalDbOptions>(builder.Configuration.GetSection("ExternalDb"));

// simulation agent client (KLÕ»OV…)
builder.Services.AddHttpClient<ISimulationExecutor, AgentSimulationExecutor>(client =>
{
  client.BaseAddress = new Uri("http://localhost:5005/");
  client.Timeout = TimeSpan.FromMinutes(5);
});

var app = builder.Build();

// middleware
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

DatabaseInitializer.EnsureImportRunTable(
  builder.Configuration.GetConnectionString("SQLite")!
);

app.Run();
