using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResortBook.Data;
using ResortBook.Models;
using ResortBook.ViewModels;

namespace ResortBook.Controllers;

public class AvailabilityController : Controller
{
    private readonly AppDbContext _db;

    public AvailabilityController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(DateTime? startDate, int days = 14)
    {
        var start = (startDate ?? DateTime.Today).Date;

        if (days < 7)
        {
            days = 7;
        }

        if (days > 31)
        {
            days = 31;
        }

        var end = start.AddDays(days);
        var dateList = Enumerable.Range(0, days)
            .Select(i => start.AddDays(i))
            .ToList();

        var rooms = await _db.Rooms
            .Where(r => r.IsActive)
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();

        var bookings = await _db.Bookings
            .Include(b => b.Room)
            .Where(b =>
                b.Status != BookingStatus.Cancelled &&
                b.CheckOutDate > start &&
                b.CheckInDate < end)
            .ToListAsync();

        var model = new AvailabilityViewModel
        {
            StartDate = start,
            EndDate = end.AddDays(-1),
            Dates = dateList,
            Rooms = rooms.Select(room => new AvailabilityRoomViewModel
            {
                RoomId = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                PricePerNight = room.PricePerNight,
                Days = dateList.Select(date =>
                {
                    var booking = bookings.FirstOrDefault(b =>
                        b.RoomId == room.Id &&
                        b.CheckInDate.Date <= date &&
                        b.CheckOutDate.Date > date);

                    return new AvailabilityDayViewModel
                    {
                        Date = date,
                        IsAvailable = booking == null,
                        StatusText = booking == null ? "Müsait" : "Dolu",
                        GuestName = booking?.GuestName,
                        BookingId = booking?.Id
                    };
                }).ToList()
            }).ToList()
        };

        return View(model);
    }
}