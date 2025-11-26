using BiographicalDetails.Infrastructure.Sql.Contexts.Loggers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BiographicalDetails.Infrastructure.Sql.Contexts.Extensions;

public static class BiographicalDataContextExtensions
{
	public static IServiceCollection AddBiographicalDetailsSqlContext(this IServiceCollection services, string? connectionString = null)
	{
		if (connectionString is null)
		{
			connectionString = DefaultConnectionString("BiographicalDetails");
		}

		services.AddDbContext<BiographicalDataDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
				options.LogTo(BiographicalDataLogger.WriteLine,
				  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
			},
			contextLifetime: ServiceLifetime.Transient,
			optionsLifetime: ServiceLifetime.Transient);

		return services;
	}

	public static string DefaultConnectionString(string dbName)
	{
		SqlConnectionStringBuilder builder = new();

		// Azure SQL Edge in Docker (locally)
		builder.DataSource = "tcp:127.0.0.1,1433";
		builder.InitialCatalog = dbName;
		builder.TrustServerCertificate = true;
		builder.MultipleActiveResultSets = true;

		builder.UserID = Environment.GetEnvironmentVariable("MY_SQL_USR");
		builder.Password = Environment.GetEnvironmentVariable("MY_SQL_PWD");

		return builder.ConnectionString;
	}
}
