using Microsoft.Extensions.DependencyInjection;
using Test.Data;
using Test.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserInfoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserInfoContext") ?? throw new InvalidOperationException("Connection string not found.")));

builder.Services.AddDbContext<SmsInfoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserInfoContext") ?? throw new InvalidOperationException("Connection string not found.")));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();


var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SignUp}/{action=Index}/{id?}");


app.UseHttpsRedirection();

app.UseStaticFiles();

//app.UseRouting();

app.UseAuthorization();



app.MapControllers();

app.Run();
