using Microsoft.EntityFrameworkCore;
using ResortBook.Data;
using ResortBook.Models;

namespace ResortBook.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _db;

    public BookingService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Aynı oda için tarih aralığı çakışması kontrolü yapar.
    /// Çakışma formülü: existing.CheckInDate < newCheckOut && newCheckIn < existing.CheckOutDate
    /// </summary>
    public async Task<bool> HasDateConflictAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
    {
        var query = _db.Bookings
            .Where(b => b.RoomId == roomId)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Where(b => b.CheckInDate < checkOut && checkIn < b.CheckOutDate);

        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookingId.Value);
        }

        return await query.AnyAsync();
    }

    public decimal CalculateTotalPrice(Room room, DateTime checkIn, DateTime checkOut)
    {
        var nights = CalculateNightCount(checkIn, checkOut);
        return room.PricePerNight * nights;
    }

    public int CalculateNightCount(DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut.Date - checkIn.Date).Days;
        return nights <= 0 ? 0 : nights;
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
    {
        var conflictRoomIds = await _db.Bookings
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Where(b => b.CheckInDate < checkOut && checkIn < b.CheckOutDate)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync();

        return await _db.Rooms
            .Where(r => r.IsActive && !conflictRoomIds.Contains(r.Id))
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();
    }
}
