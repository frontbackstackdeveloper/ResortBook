using ResortBook.Models;

namespace ResortBook.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (!db.Rooms.Any())
        {
            db.Rooms.AddRange(
                new Room { RoomNumber = "101", RoomType = "Standard", Capacity = 2, PricePerNight = 2200, Description = "Bahçe manzaralı standart oda" },
                new Room { RoomNumber = "102", RoomType = "Standard", Capacity = 2, PricePerNight = 2200, Description = "Havuz tarafı standart oda" },
                new Room { RoomNumber = "201", RoomType = "Deluxe", Capacity = 3, PricePerNight = 3400, Description = "Deniz manzaralı deluxe oda" },
                new Room { RoomNumber = "301", RoomType = "Suite", Capacity = 4, PricePerNight = 5200, Description = "Aile kullanımına uygun suite oda" }
            );
            db.SaveChanges();
        }

        if (!db.Bookings.Any())
        {
            var firstRoom = db.Rooms.First();
            db.Bookings.Add(new Booking
            {
                RoomId = firstRoom.Id,
                GuestName = "Demo Misafir",
                GuestEmail = "demo@example.com",
                GuestPhone = "+90 555 000 00 00",
                CheckInDate = DateTime.Today.AddDays(3),
                CheckOutDate = DateTime.Today.AddDays(6),
                Status = BookingStatus.Confirmed,
                PaymentStatus = PaymentStatus.Paid,
                TotalPrice = firstRoom.PricePerNight * 3,
                Notes = "Seed demo rezervasyonu"
            });
            db.SaveChanges();
        }
    }
}
