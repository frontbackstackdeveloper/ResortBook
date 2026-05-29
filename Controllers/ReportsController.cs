using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResortBook.Data;
using ResortBook.Models;
using ResortBook.ViewModels;

namespace ResortBook.Controllers;

public class ReportsController : Controller
{
    private readonly AppDbContext _db;

    public ReportsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var totalRooms = await _db.Rooms.CountAsync(r => r.IsActive);

        var activeBookingCount = await _db.Bookings
            .CountAsync(b => b.Status == BookingStatus.Confirmed);

        var roomTypeRows = await _db.Bookings
            .Include(b => b.Room)
            .Where(b => b.Room != null)
            .GroupBy(b => b.Room!.RoomType)
            .Select(g => new { RoomType = g.Key, Count = g.Count() })
            .ToListAsync();

        // Toplam gelir sadece Confirmed + Paid rezervasyonlardan hesaplanýr.
        // SQLite decimal SumAsync desteklemediði için önce listeye çekiyoruz.
        var revenueBookings = await _db.Bookings
            .Where(b =>
                b.Status == BookingStatus.Confirmed &&
                b.PaymentStatus == PaymentStatus.Paid)
            .ToListAsync();

        var totalRevenue = revenueBookings.Sum(b => b.TotalPrice);

        var model = new ReportViewModel
        {
            TotalBookings = await _db.Bookings.CountAsync(),

            PendingBookings = await _db.Bookings
                .CountAsync(b => b.Status == BookingStatus.Pending),

            ConfirmedBookings = await _db.Bookings
                .CountAsync(b => b.Status == BookingStatus.Confirmed),

            CancelledBookings = await _db.Bookings
                .CountAsync(b => b.Status == BookingStatus.Cancelled),

            TotalRevenue = totalRevenue,

            OccupancyRate = totalRooms == 0
                ? 0
                : Math.Round((double)activeBookingCount / totalRooms * 100, 2),

            BookingsByRoomType = roomTypeRows.ToDictionary(x => x.RoomType, x => x.Count)
        };

        return View(model);
    }
}