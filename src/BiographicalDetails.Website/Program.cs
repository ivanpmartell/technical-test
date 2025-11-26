using BiographicalDetails.Application.Services;
using BiographicalDetails.Application.Validators;
using BiographicalDetails.Domain.Abstractions;
using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.EntityModels.Mappers;
using BiographicalDetails.Infrastructure.InMemory;
using BiographicalDetails.Infrastructure.Sql;
using BiographicalDetails.Infrastructure.Sql.Contexts.Extensions;
using BiographicalDetails.Website.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Our services
var biographicalDataConnectionString = builder.Configuration.GetConnectionString("BiographicalDataConnection")
	?? throw new InvalidOperationException("Connection string 'BiographicalDataConnection' not found.");
SqlConnectionStringBuilder sql = new(biographicalDataConnectionString);
sql.IntegratedSecurity = false;
sql.UserID = Environment.GetEnvironmentVariable("MY_SQL_USR");
sql.Password = Environment.GetEnvironmentVariable("MY_SQL_PWD");
builder.Services.AddBiographicalDetailsSqlContext(sql.ConnectionString);

builder.Services.AddScoped<IBiographicalDataRepository, SqlBiographicalDataRepository>();
// Can also change to In memory database (different db schema)
//builder.Services.AddScoped<IBiographicalDataRepository, InMemoryBiographicalDataRepository>();

builder.Services.AddScoped<IBiographicalDataMapper, BiographicalDataMapper>();
builder.Services.AddScoped<IValidatorSIN, SINValidator>();
builder.Services.AddScoped<IValidatorUCI, UCIValidator>();
builder.Services.AddScoped<IBiographicalDataValidator, BiographicalDataValidator>();
builder.Services.AddScoped<BiographicalDetailsService>();
// End of our services

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();
app.MapRazorPages()
	.WithStaticAssets();

app.Run();