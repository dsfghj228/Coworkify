using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext)
    : IdentityDbContext<AppUser>(dbContext)
{
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Workspace>()
            .HasOne(w => w.Owner)
            .WithMany(w => w.Workspaces)
            .HasForeignKey(w => w.OwnerId);
        
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Workspace)
            .WithMany(w => w.Rooms)
            .HasForeignKey(r => r.WorkspaceId);
        
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.UserId);
        
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.RoomId);
        
        modelBuilder.Entity<Room>()
            .Property(r => r.HourlyRate)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)");

    }
}
    