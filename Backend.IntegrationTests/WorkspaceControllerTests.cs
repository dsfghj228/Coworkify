using System.Net;
using System.Net.Http.Json;
using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.IntegrationTests;

public class WorkspaceControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private AppUser _testUser;
    private string _token;

    public WorkspaceControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        (_testUser, _token) = await CreateTestUserAsync();

        await CreateTestWorkspaceAsync(_token, "Workspace1");
        await CreateTestWorkspaceAsync(_token, "Workspace2");
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }
    
    private async Task<(AppUser user, string token)> CreateTestUserAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        var testUser = new AppUser
        {
            UserName = $"TestUser_{Guid.NewGuid()}",
            Email = $"test_{Guid.NewGuid()}@example.com"
        };

        var result = await userManager.CreateAsync(testUser, "P@ssw0rd!");
        if (!result.Succeeded)
            throw new Exception("Не удалось создать тестового пользователя");

        var token = tokenService.CreateToken(testUser);
        return (testUser, token);
    }

    private async Task<HttpResponseMessage> CreateTestWorkspaceAsync(string token, string name)
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var workspace = new CreateWorkspace
        {
            Name = name,
            Description = $"Description for {name}",
            Address = "TestAddress"
        };

        return await _client.PostAsJsonAsync("api/workspace/create", workspace);
    }

    [Fact]
    public async Task CreateWorkspace_ShouldReturnNewWorkspace()
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        var response = await CreateTestWorkspaceAsync(_token, "MyWorkspaceTest");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
    }
}
