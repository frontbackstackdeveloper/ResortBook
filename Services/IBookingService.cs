using ResortBook.Models;

namespace ResortBook.Services;

public interface IBookingService
{
    Task<bool> HasDateConflictAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);
    decimal CalculateTotalPrice(Room room, DateTime checkIn, DateTime checkOut);
    int CalculateNightCount(DateTime checkIn, DateTime checkOut);
    Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
}
