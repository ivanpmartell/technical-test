using BiographicalDetails.Application.Validators;
using BiographicalDetails.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BiographicalDetails.Application.Services.Extensions;

public static class BiographicalDetailsServiceExtensions
{
	public static IServiceCollection AddBiographicalDetailsService(this IServiceCollection services)
	{
		services.AddKeyedSingleton<IStringValidator, SINValidator>("sin");
		services.AddKeyedSingleton<IStringValidator, UCIValidator>("uci");
		services.AddScoped<IBiographicalDataValidator, BiographicalDataValidator>();
		services.AddScoped<BiographicalDetailsService>();
		return services;
	}
}
