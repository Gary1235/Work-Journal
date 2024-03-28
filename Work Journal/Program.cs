using Microsoft.EntityFrameworkCore;
using Models.Models;
using Newtonsoft.Json.Serialization;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add ConnectionString from appsetting.json
var connectionString = builder.Configuration.GetConnectionString("DB");
builder.Services.AddDbContext<WorkJournalContext>(options => options.UseSqlServer(connectionString));

// Register interface
builder.Services.AddScoped<IWorkScheduleService, WorkScheduleService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Set default JSON library to NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
