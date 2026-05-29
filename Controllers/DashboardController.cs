using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResortBook.Data;
using ResortBook.Models;
using ResortBook.ViewModels;

namespace ResortBook.Controllers;

public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

        var totalRooms = await _db.Rooms.CountAsync();
        var activeRooms = await _db.Rooms.CountAsync(r => r.IsActive);
        var totalBookings = await _db.Bookings.CountAsync();

        var confirmedBookingsCount = await _db.Bookings
            .CountAsync(b => b.Status == BookingStatus.Confirmed);

        // Gelir sadece onaylanmýþ ve ödenmiþ rezervasyonlardan hesaplanýr.
        // SQLite decimal SumAsync desteklemediði için önce listeye çekiyoruz.
        var monthlyRevenueBookings = await _db.Bookings
            .Where(b =>
                b.CreatedAt >= firstDayOfMonth &&
                b.Status == BookingStatus.Confirmed &&
                b.PaymentStatus == PaymentStatus.Paid)
            .ToListAsync();

        var monthlyRevenue = monthlyRevenueBookings.Sum(b => b.TotalPrice);

        var model = new DashboardViewModel
        {
            TotalRooms = totalRooms,
            ActiveRooms = activeRooms,
            TotalBookings = totalBookings,

            PendingBookings = await _db.Bookings
                .CountAsync(b => b.Status == BookingStatus.Pending),

            ConfirmedBookings = confirmedBookingsCount,

            CancelledBookings = await _db.Bookings
                .CountAsync(b => b.Status == BookingStatus.Cancelled),

            MonthlyRevenue = monthlyRevenue,

            OccupancyRate = totalRooms == 0
                ? 0
                : Math.Round((double)confirmedBookingsCount / Math.Max(totalRooms, 1) * 100, 2),

            LatestBookings = await _db.Bookings
                .Include(b => b.Room)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}