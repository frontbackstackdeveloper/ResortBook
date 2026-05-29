using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResortBook.Data;
using ResortBook.Models;
using ResortBook.Services;
using ResortBook.ViewModels;

namespace ResortBook.Controllers;

public class BookingsController : Controller
{
    private readonly AppDbContext _db;
    private readonly IBookingService _bookingService;
    private readonly IEmailLogService _emailLogService;

    public BookingsController(
        AppDbContext db,
        IBookingService bookingService,
        IEmailLogService emailLogService)
    {
        _db = db;
        _bookingService = bookingService;
        _emailLogService = emailLogService;
    }

    public async Task<IActionResult> Index()
    {
        // Önceden onaylanmış ama ödeme durumu Unpaid kalmış kayıtları düzeltir.
        // Böylece Confirmed rezervasyonlar sistemde Paid olarak görünür.
        var confirmedUnpaidBookings = await _db.Bookings
            .Where(b => b.Status == BookingStatus.Confirmed && b.PaymentStatus == PaymentStatus.Unpaid)
            .ToListAsync();

        if (confirmedUnpaidBookings.Any())
        {
            foreach (var booking in confirmedUnpaidBookings)
            {
                booking.PaymentStatus = PaymentStatus.Paid;
            }

            await _db.SaveChangesAsync();
        }

        var bookings = await _db.Bookings
            .Include(b => b.Room)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return View(bookings);
    }

    public async Task<IActionResult> Details(int id)
    {
        var booking = await _db.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound();
        }

        return View(booking);
    }

    public async Task<IActionResult> Create()
    {
        var model = new BookingCreateViewModel
        {
            Rooms = await GetRoomSelectListAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingCreateViewModel model)
    {
        if (model.CheckOutDate <= model.CheckInDate)
        {
            ModelState.AddModelError(nameof(model.CheckOutDate), "Çıkış tarihi giriş tarihinden sonra olmalıdır.");
        }

        var room = await _db.Rooms.FindAsync(model.RoomId);

        if (room == null)
        {
            ModelState.AddModelError(nameof(model.RoomId), "Seçilen oda bulunamadı.");
        }

        if (room != null && await _bookingService.HasDateConflictAsync(model.RoomId, model.CheckInDate, model.CheckOutDate))
        {
            ModelState.AddModelError(string.Empty, "Seçilen oda bu tarih aralığında doludur. Double-booking engellendi.");
        }

        if (!ModelState.IsValid)
        {
            model.Rooms = await GetRoomSelectListAsync();
            return View(model);
        }

        var booking = new Booking
        {
            RoomId = model.RoomId,
            GuestName = model.GuestName,
            GuestEmail = model.GuestEmail,
            GuestPhone = model.GuestPhone,
            CheckInDate = model.CheckInDate.Date,
            CheckOutDate = model.CheckOutDate.Date,
            Notes = model.Notes,
            Status = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.Unpaid,
            TotalPrice = _bookingService.CalculateTotalPrice(room!, model.CheckInDate, model.CheckOutDate),
            CreatedAt = DateTime.Now
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        _emailLogService.AddBookingCreatedLog(booking, room!.RoomNumber);

        TempData["Success"] = "Rezervasyon oluşturuldu. Misafir ve admin için otomatik e-posta bildirimi üretildi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(int id)
    {
        var booking = await _db.Bookings.FindAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        booking.Status = BookingStatus.Confirmed;
        booking.PaymentStatus = PaymentStatus.Paid;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Rezervasyon onaylandı ve ödeme durumu Paid olarak güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _db.Bookings.FindAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        booking.Status = BookingStatus.Cancelled;
        booking.PaymentStatus = PaymentStatus.Unpaid;

        await _db.SaveChangesAsync();

        TempData["Warning"] = "Rezervasyon iptal edildi.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetRoomSelectListAsync()
    {
        return await _db.Rooms
            .Where(r => r.IsActive)
            .OrderBy(r => r.RoomNumber)
            .Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"{r.RoomNumber} - {r.RoomType} ({r.PricePerNight:N0} TL/gece)"
            })
            .ToListAsync();
    }
}