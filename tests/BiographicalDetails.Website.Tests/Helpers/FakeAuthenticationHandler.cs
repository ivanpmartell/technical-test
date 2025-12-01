using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BiographicalDetails.Website.Tests.Helpers;

public class FakeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public FakeAuthenticationHandler(
		IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		System.Text.Encodings.Web.UrlEncoder encoder)
		: base(options, logger, encoder) { }

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var claims = new[]
		{
			new Claim(ClaimTypes.Name, "ivan@test.com")
		};

		var identity = new ClaimsIdentity(claims, "FakeAuth");
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, "FakeAuth");

		return Task.FromResult(AuthenticateResult.Success(ticket));
	}
}