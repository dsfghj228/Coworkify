using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Backend.Data;
using Backend.Dto.WorkspaceDto;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.IntegrationTests;

[Collection("Sequential")]
public class WorkspaceControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private AppUser _testUser;
    private string _token;
    private readonly Guid workspaceId;

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
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Workspaces.RemoveRange(context.Workspaces);
        context.Users.RemoveRange(context.Users);
        await context.SaveChangesAsync();
        _client.Dispose();
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
    public async Task CreateWorkspace_WhenAuthorized_ShouldReturnNewWorkspace()
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        var response = await CreateTestWorkspaceAsync(_token, "MyWorkspaceTest");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
    }

    [Fact]
    public async Task CreateWorkspace_WhenNotAuthorized_ShouldThrowUnauthorizedException()
    {
        var workspace = new CreateWorkspace
        {
            Name = "TestWorkspace",
            Description = $"Description for TestWorkspace",
            Address = "TestAddress"
        };
        _client.DefaultRequestHeaders.Authorization = null;
        
        var response = await _client.PostAsJsonAsync("api/workspace/create", workspace);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAllWorkspaces_ShouldReturnAllExistingWorkspaces()
    {
        var response = await _client.GetAsync("/api/workspace");
        var content = await response.Content.ReadAsStringAsync();
        var workspaces = await response.Content.ReadFromJsonAsync<List<Workspace>>();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.NotNull(workspaces);
        Assert.Equal(2, workspaces.Count);
    }
}

[CollectionDefinition("Sequential", DisableParallelization = true)]
public class SequentialCollectionDefinition { }