using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.EntityModels.Mappers;
using BiographicalDetails.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Environment;

namespace BiographicalDetails.Infrastructure.Sqlite.Contexts.Extensions;

public static class BiographicalDataContextExtensions
{
	public static IServiceCollection AddBiographicalDetailsSqliteContext(this IServiceCollection services, string? connectionString = null)
	{
		connectionString ??= DefaultConnectionString("BiographicalDetails");

		var logger = new BiographicalDataLogger
		{
			FolderName = "sqlite-logs"
		};

		services.AddDbContext<BiographicalDataDbContext>(options =>
		{
			options.UseSqlite(connectionString);
			options.LogTo(logger.WriteLine,
			  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
		},
			contextLifetime: ServiceLifetime.Transient,
			optionsLifetime: ServiceLifetime.Transient);

		services.AddScoped<IBiographicalDataMapper, BiographicalDataEntityMapper>();
		return services;
	}

	public static string DefaultConnectionString(string dbName)
	{
		var fullPath = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), $"{dbName}.db");
		return $"DataSource={fullPath}";
	}
}
