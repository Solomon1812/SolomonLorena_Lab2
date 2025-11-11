using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Solomon_Lorena_Lab2.Data;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
   policy.RequireRole("Admin"));
});


// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Books");
    options.Conventions.AuthorizeFolder("/Publishers");
    options.Conventions.AuthorizeFolder("/Categories");
    //for Authorizing the folders  - no login, no access


    options.Conventions.AllowAnonymousToPage("/Books/Index");
    options.Conventions.AllowAnonymousToPage("/Books/Details");
    //allowing anonymous access to Index and Details pages in Books folder

    options.Conventions.AuthorizeFolder("/Members", "AdminPolicy"); //members area restricted to Admins only
});

builder.Services.AddDbContext<Solomon_Lorena_Lab2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Solomon_Lorena_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Solomon_Lorena_Lab2Context' not found.")));


// Register the new Identity Context ( Lab 5, Step 16)
builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Solomon_Lorena_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Solomon_Lorena_Lab2Context' not found.")));


builder.Services.AddDefaultIdentity<IdentityUser>(options =>
options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() //for admin / user roles
    .AddEntityFrameworkStores<LibraryIdentityContext>();


//// 2. Configure Identity to use the new Context (Lab 5, Step 16)
//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    // Temporarily disable confirmation for testing registration flow
//    options.SignIn.RequireConfirmedAccount = false;

//    // --- RELAXED PASSWORD REQUIREMENTS  ---
//    options.Password.RequireDigit = false;          // No number required
//    options.Password.RequireLowercase = false;      // No lowercase required
//    options.Password.RequireUppercase = false;      // No uppercase required
//    options.Password.RequireNonAlphanumeric = false; // No special character required
//    options.Password.RequiredLength = 4;            // Minimum length of 4 characters
//})
//    .AddEntityFrameworkStores<LibraryIdentityContext>();



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
