using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class AppUser : IdentityUser
{
    public List<Workspace> Workspaces { get; set; } = new();
    public List<Booking> Bookings { get; set; } = new();
}