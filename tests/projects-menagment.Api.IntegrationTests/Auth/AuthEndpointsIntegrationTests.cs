using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using projects_menagment.Api.IntegrationTests.Infrastructure;
using projects_menagment.Infrastructure.Persistence;
using Xunit;

namespace projects_menagment.Api.IntegrationTests.Auth;

public sealed class AuthEndpointsIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthEndpointsIntegrationTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Signup_CreatesUserInDatabase()
    {
        var request = new
        {
            firstName = "John",
            lastName = "Doe",
            email = "john.integration@test.com",
            password = "Valid1!Pass"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/signup", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var created = db.Users.SingleOrDefault(x => x.Email == "john.integration@test.com");

        Assert.NotNull(created);
        Assert.Equal("John", created!.FirstName);
        Assert.Equal("Doe", created.LastName);
    }

    [Fact]
    public async Task Login_ReturnsTokensAndPersistsRefreshToken()
    {
        var signup = new
        {
            firstName = "Ana",
            lastName = "Smith",
            email = "ana.integration@test.com",
            password = "Valid1!Pass"
        };
        await _client.PostAsJsonAsync("/api/auth/signup", signup);

        var login = new
        {
            email = "ana.integration@test.com",
            password = "Valid1!Pass"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<LoginResponseContract>();
        Assert.NotNull(payload);
        Assert.False(string.IsNullOrWhiteSpace(payload!.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(payload.RefreshToken));

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var tokenInDb = db.RefreshTokens.SingleOrDefault(x => x.Token == payload.RefreshToken);
        Assert.NotNull(tokenInDb);
    }

    [Fact]
    public async Task Refresh_RotatesRefreshToken()
    {
        var signup = new
        {
            firstName = "Mia",
            lastName = "Jones",
            email = "mia.integration@test.com",
            password = "Valid1!Pass"
        };
        await _client.PostAsJsonAsync("/api/auth/signup", signup);

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "mia.integration@test.com",
            password = "Valid1!Pass"
        });
        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<LoginResponseContract>();
        Assert.NotNull(loginPayload);

        var refreshResponse = await _client.PostAsJsonAsync("/api/auth/refresh", new
        {
            refreshToken = loginPayload!.RefreshToken
        });

        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
        var refreshPayload = await refreshResponse.Content.ReadFromJsonAsync<LoginResponseContract>();
        Assert.NotNull(refreshPayload);
        Assert.NotEqual(loginPayload.RefreshToken, refreshPayload!.RefreshToken);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var oldToken = db.RefreshTokens.Single(x => x.Token == loginPayload.RefreshToken);
        var newToken = db.RefreshTokens.SingleOrDefault(x => x.Token == refreshPayload.RefreshToken);

        Assert.True(oldToken.IsRevoked);
        Assert.NotNull(newToken);
        Assert.False(newToken!.IsRevoked);
    }

    private sealed record LoginResponseContract(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresAt,
        DateTime RefreshTokenExpiresAt);
}
