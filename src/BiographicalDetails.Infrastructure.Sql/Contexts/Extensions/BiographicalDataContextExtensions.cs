using BiographicalDetails.Domain.Abstractions;
using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.EntityModels.Mappers;
using BiographicalDetails.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BiographicalDetails.Infrastructure.Sql.Contexts.Extensions;

public static class BiographicalDataContextExtensions
{
	public static IServiceCollection AddBiographicalDetailsSqlContext(this IServiceCollection services, string? connectionString = null)
	{
		connectionString ??= DefaultConnectionString("BiographicalDetails");

		var logger = new BiographicalDataLogger
		{
			FolderName = "sql-logs"
		};

		services.AddDbContext<BiographicalDataDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
				options.LogTo(logger.WriteLine,
				  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
			},
			contextLifetime: ServiceLifetime.Transient,
			optionsLifetime: ServiceLifetime.Transient);

		services.AddScoped<IBiographicalDataRepository, SqlBiographicalDataRepository>();
		services.AddScoped<IBiographicalDataMapper, BiographicalDataEntityMapper>();
		return services;
	}

	public static string DefaultConnectionString(string dbName)
	{
		SqlConnectionStringBuilder builder = new()
		{
			// Azure SQL Edge in Docker (locally)
			DataSource = "tcp:127.0.0.1,1433",
			InitialCatalog = dbName,
			TrustServerCertificate = true,
			MultipleActiveResultSets = true,

			UserID = Environment.GetEnvironmentVariable("MY_SQL_USR"),
			Password = Environment.GetEnvironmentVariable("MY_SQL_PWD")
		};

		return builder.ConnectionString;
	}
}
