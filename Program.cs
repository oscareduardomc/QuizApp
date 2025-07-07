// Program.cs
using BlazorQuizApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection; // ¡AÑADE ESTA LÍNEA!

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configura el DbContext para usar PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// *******************************************************************
// ¡AÑADE ESTAS LÍNEAS PARA CONFIGURAR DATA PROTECTION!
builder.Services.AddDataProtection()
    .PersistKeysToDbContext<ApplicationDbContext>();
// *******************************************************************

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Opcional: Para aplicar migraciones y sembrar datos al inicio (solo para desarrollo)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // dbContext.Database.Migrate(); // Esta línea DEBE seguir comentada.
}

app.Run();