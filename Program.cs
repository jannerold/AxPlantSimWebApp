using AxPlantSimWebApp.Data;
using AxPlantSimWebApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(
  options => options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"))
);

// naše services
builder.Services.AddScoped<TableBrowserService>();

var app = builder.Build();

// standardní middleware
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

app.Run();
