using Backend.MediatR.Commands.Room;
using Backend.Models;

namespace Backend.Interfaces;

public interface IRoomRepository
{
    Task<Room> CreateRoom(CreateRoomCommand request);
    Task<Room> GetRoomById(Guid id);
    Task<List<Room>> GetAllRooms(Guid workspaceId);
}