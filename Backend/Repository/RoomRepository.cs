using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.MediatR.Commands.Room;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    public async Task<Room> CreateRoom(CreateRoomCommand request)
    {
        var workspace = await context.Workspaces
            .Include(w => w.Rooms)
            .FirstOrDefaultAsync(w => w.Id == request.WorkspaceId);
        if (workspace == null)
        {
            throw new CustomExceptions.WorkspaceNotFoundException(request.WorkspaceId);
        }

        var room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            HourlyRate = request.HourlyRate,
            WorkspaceId = request.WorkspaceId,
            Workspace = workspace,
            Bookings = new List<Booking>()
        };

        workspace.Rooms.Add(room);
        await context.Rooms.AddAsync(room);
        await context.SaveChangesAsync();
        return room;
    }

    public async Task<Room> GetRoomById(Guid id)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(w => w.Id == id);
        if (room == null)
        {
            throw new CustomExceptions.RoomNotFoundException(id);
        }
        
        return room;
    }

    public async Task<List<Room>> GetAllRooms(Guid workspaceId)
    {
        return await context.Rooms
            .Where(r => r.WorkspaceId == workspaceId)
            .ToListAsync();
    }

    public async Task<Room> DeleteRoom(Guid id)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(w => w.Id == id);
        if (room == null)
        {
            throw new CustomExceptions.RoomNotFoundException(id);
        }
        context.Rooms.Remove(room);
        await context.SaveChangesAsync();
        return room;
    }
}