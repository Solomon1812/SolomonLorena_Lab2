using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Solomon_Lorena_Lab2.Data;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Solomon_Lorena_Lab2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Solomon_Lorena_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Solomon_Lorena_Lab2Context' not found.")));


// 1. Register the new Identity Context (from Lab 5, Step 16)
builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Solomon_Lorena_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Solomon_Lorena_Lab2Context' not found.")));

// 2. Configure Identity to use the new Context (from Lab 5, Step 16)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LibraryIdentityContext>();



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

app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context>();
    context.Database.Migrate();

    if (!context.Publisher.Any())
    {
        context.Publisher.AddRange(
            new Solomon_Lorena_Lab2.Models.Publisher { PublisherName = "Humanitas" },
            new Solomon_Lorena_Lab2.Models.Publisher { PublisherName = "ART" },
            new Solomon_Lorena_Lab2.Models.Publisher { PublisherName = "Litera" }
        );
        context.SaveChanges();
    }
}

app.Run();
