using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResortBook.Data;
using ResortBook.Models;
using ResortBook.ViewModels;

namespace ResortBook.Controllers;

public class PaymentsController : Controller
{
    private readonly AppDbContext _db;

    public PaymentsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var unpaidBookings = await _db.Bookings
            .Include(b => b.Room)
            .Where(b => b.PaymentStatus == PaymentStatus.Unpaid && b.Status != BookingStatus.Cancelled)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return View(unpaidBookings);
    }

    public async Task<IActionResult> Pay(int id)
    {
        var booking = await _db.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound();
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            TempData["Warning"] = "İptal edilmiş rezervasyon için ödeme alınamaz.";
            return RedirectToAction(nameof(Index));
        }

        if (booking.PaymentStatus == PaymentStatus.Paid)
        {
            TempData["Success"] = "Bu rezervasyonun ödemesi zaten alınmış.";
            return RedirectToAction(nameof(Index));
        }

        var model = new PaymentViewModel
        {
            BookingId = booking.Id,
            GuestName = booking.GuestName,
            RoomInfo = $"{booking.Room?.RoomNumber} - {booking.Room?.RoomType}",
            Amount = booking.TotalPrice,
            CardHolderName = booking.GuestName
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pay(PaymentViewModel model)
    {
        var booking = await _db.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == model.BookingId);

        if (booking == null)
        {
            return NotFound();
        }

        model.GuestName = booking.GuestName;
        model.RoomInfo = $"{booking.Room?.RoomNumber} - {booking.Room?.RoomType}";
        model.Amount = booking.TotalPrice;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            TempData["Warning"] = "İptal edilmiş rezervasyon için ödeme alınamaz.";
            return RedirectToAction(nameof(Index));
        }

        booking.PaymentStatus = PaymentStatus.Paid;
        booking.Status = BookingStatus.Confirmed;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Demo ödeme başarıyla tamamlandı. Kart bilgisi saklanmadı. Rezervasyon Onaylandı ve Ödendi olarak güncellendi.";
        return RedirectToAction(nameof(Index));
    }
}