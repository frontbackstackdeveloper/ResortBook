using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResortBook.Data;
using ResortBook.Models;

namespace ResortBook.Controllers;

public class RoomsController : Controller
{
    private readonly AppDbContext _db;

    public RoomsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _db.Rooms.OrderBy(r => r.RoomNumber).ToListAsync();
        return View(rooms);
    }

    public async Task<IActionResult> Details(int id)
    {
        var room = await _db.Rooms
            .Include(r => r.Bookings.OrderByDescending(b => b.CreatedAt))
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null) return NotFound();
        return View(room);
    }

    public IActionResult Create()
    {
        return View(new Room());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Room room)
    {
        if (!ModelState.IsValid) return View(room);
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Oda başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        if (room == null) return NotFound();
        return View(room);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Room room)
    {
        if (id != room.Id) return BadRequest();
        if (!ModelState.IsValid) return View(room);

        _db.Update(room);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Oda bilgileri güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        if (room == null) return NotFound();

        var hasBooking = await _db.Bookings.AnyAsync(b => b.RoomId == id);
        if (hasBooking)
        {
            room.IsActive = false;
            TempData["Warning"] = "Bu odanın rezervasyon geçmişi olduğu için silinmedi, pasif hale getirildi.";
        }
        else
        {
            _db.Rooms.Remove(room);
            TempData["Success"] = "Oda silindi.";
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
