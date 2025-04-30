using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PickMe.Business;
using PickMe.Business.Services.Abstractions;
using PickMe.Business.Services.Concretes;
using PickMe.Core.Models;
using PickMe.Data;
using PickMe.Data.Repositories.Abstracts;
using PickMe.Data.Repositories.Concretes;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection3")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure Repository and Service Dependencies

builder.Services.AddDataService();
builder.Services.AddBusinessService(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithRedirects("/Home/Error?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
