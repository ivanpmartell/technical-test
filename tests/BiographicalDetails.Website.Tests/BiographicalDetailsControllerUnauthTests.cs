using BiographicalDetails.Infrastructure.Sqlite.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BiographicalDetails.Website.Tests;

public class UnauthenticatedWebAppFixture : IDisposable
{
	internal HttpClient client;
	internal WebApplicationFactory<Program> factory;

	public UnauthenticatedWebAppFixture()
	{
		factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
		{
			builder.ConfigureServices(services =>
			{
				var sp = services.BuildServiceProvider();
				using (var scope = sp.CreateScope())
				{
					var db = scope.ServiceProvider.GetRequiredService<BiographicalDataDbContext>();
					db.Database.Migrate();
				}
			});
		});
		client = factory.CreateClient(
			new WebApplicationFactoryClientOptions { AllowAutoRedirect = false }
		);
	}

	public void Dispose()
	{
		client.Dispose();
		factory.Dispose();
	}
}

public class BiographicalDetailsControllerUnauthTests : IClassFixture<UnauthenticatedWebAppFixture>
{
	private UnauthenticatedWebAppFixture _webappFixture;

	public BiographicalDetailsControllerUnauthTests(UnauthenticatedWebAppFixture appFixture)
	{
		_webappFixture = appFixture;
	}

	[Fact]
	public async Task Index_WithUnauthenticatedUser_Unsuccessful()
	{
		var response = await _webappFixture.client.GetAsync("/BiographicalDetails");
		Assert.False(response.IsSuccessStatusCode);
	}
}
