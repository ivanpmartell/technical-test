using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using BiographicalDetails.Infrastructure.Sql.Contexts;
using Microsoft.EntityFrameworkCore;
using BiographicalDetails.Website.Tests.Helpers;
using Microsoft.AspNetCore.Authentication;

namespace BiographicalDetails.Website.Tests;

public class AuthenticatedWebappFixture : IDisposable
{
	internal HttpClient client;
	internal WebApplicationFactory<Program> factory;

	public AuthenticatedWebappFixture()
	{
		factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
		{
			builder.ConfigureServices(services =>
			{
				services.AddAuthentication("FakeAuth")
				    .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("FakeAuth", options => { } );

				var sp = services.BuildServiceProvider();
				using var scope = sp.CreateScope();
				var db = scope.ServiceProvider.GetRequiredService<BiographicalDataDbContext>();
				db.Database.Migrate();
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
		GC.SuppressFinalize(this);
	}
}

public class BiographicalDetailsControllerAuthTests : IClassFixture<AuthenticatedWebappFixture>
{
	private readonly AuthenticatedWebappFixture _webappFixture;

	public BiographicalDetailsControllerAuthTests(AuthenticatedWebappFixture appFixture)
	{
		_webappFixture = appFixture;
	}

	[Fact]
	public async Task Index_WithAuthenticatedUser_Success()
	{
		var response = await _webappFixture.client.GetAsync("/BiographicalDetails");
		response.EnsureSuccessStatusCode();
	}
}
