using BiographicalDetails.Infrastructure.Sqlite.Contexts.Loggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Environment;

namespace BiographicalDetails.Infrastructure.Sqlite.Contexts.Extensions;

public static class BiographicalDataContextExtensions
{
	public static IServiceCollection AddBiographicalDetailsSqliteContext(this IServiceCollection services, string? connectionString = null)
	{
		if (connectionString is null)
		{
			connectionString = DefaultConnectionString("BiographicalDetails");
		}

		services.AddDbContext<BiographicalDataDbContext>(options =>
		{
			options.UseSqlite(connectionString);
			options.LogTo(BiographicalDataLogger.WriteLine,
			  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
		},
			contextLifetime: ServiceLifetime.Transient,
			optionsLifetime: ServiceLifetime.Transient);

		return services;
	}

	public static string DefaultConnectionString(string dbName)
	{
		var fullPath = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), $"{dbName}.db");
		return $"DataSource={fullPath}";
	}
}
