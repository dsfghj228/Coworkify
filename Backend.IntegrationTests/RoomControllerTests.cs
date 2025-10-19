using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Backend.Data;
using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Room;
using Backend.MediatR.Queries.Room;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.IntegrationTests;

[Collection("SequentialWorkspace")]
public class RoomControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private AppUser _testUser;
    private string _token;
    private Guid _roomId;
    private Guid _workspaceId;

    public RoomControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    public async Task InitializeAsync()
    {
        (_testUser, _token) = await CreateTestUserAsync();
        
        await CreateTestWorkspaceAsync(_token, "testWorkspace");
        await CreateTestRoomAsync(_token, "testRoom");
    }

    public async Task DisposeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Workspaces.RemoveRange(context.Workspaces);
        context.Rooms.RemoveRange(context.Rooms);
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
    
    private async Task CreateTestWorkspaceAsync(string token, string name)
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var workspace = new CreateWorkspace
        {
            Name = name,
            Description = $"Description for {name}",
            Address = "TestAddress"
        };

        var response = await _client.PostAsJsonAsync("api/workspace/create", workspace);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Workspace>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        _workspaceId = result.Id;
    }

    private async Task<HttpResponseMessage> CreateTestRoomAsync(string token, string name)
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var room = new CreateRoomCommand
        {
            Name = name,
            Capacity = 10,
            HourlyRate = 10,
            WorkspaceId = _workspaceId
        };
        var response = await _client.PostAsJsonAsync("api/room", room);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Room>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        _roomId = result.Id;
        return response;
    }

    [Fact]
    public async Task CreateRoom_WhenAuthorized_ShouldReturnNewRoom()
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        
        var response = await CreateTestRoomAsync(_token, "MyRoomTest");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
    }
    
    [Fact]
    public async Task CreateRoom_WhenNotAuthorized_ShouldReturnNewRoom()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var room = new CreateRoomCommand
        {
            Name = "Test",
            Capacity = 10,
            HourlyRate = 10,
            WorkspaceId = _workspaceId
        };
        var response = await _client.PostAsJsonAsync("api/room", room);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetRoomById_WhenRoomExists_ShouldReturnRoom()
    {
        var response = await _client.GetAsync($"api/room/{_roomId}");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GetAllRoomsQuery>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.Equal(_workspaceId, result.WorkspaceId);
    }
    
    [Fact]
    public async Task GetRoomById_WhenRoomDoesNotExists_ShouldReturnRoom()
    {
        var roomId = Guid.NewGuid();
        var response = await _client.GetAsync($"api/room/{roomId}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

[CollectionDefinition("SequentialRoom", DisableParallelization = true)]
public class SequentialRoomCollectionDefinition { }