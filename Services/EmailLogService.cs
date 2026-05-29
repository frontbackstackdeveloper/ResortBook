using ResortBook.Models;

namespace ResortBook.Services;

public interface IEmailLogService
{
    void AddBookingCreatedLog(Booking booking, string roomNumber);
    IReadOnlyList<EmailLogItem> GetLogs();
}

public class EmailLogService : IEmailLogService
{
    private readonly List<EmailLogItem> _logs = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public void AddBookingCreatedLog(Booking booking, string roomNumber)
    {
        lock (_lock)
        {
            _logs.Insert(0, new EmailLogItem
            {
                Id = _nextId++,
                CreatedAt = DateTime.Now,
                RecipientType = "Misafir",
                RecipientEmail = booking.GuestEmail,
                Subject = "ResortBook Rezervasyon Bilgilendirmesi",
                Body = $"{booking.GuestName} için {roomNumber} numaralı oda rezervasyonu oluşturuldu. Tarih: {booking.CheckInDate:dd.MM.yyyy} - {booking.CheckOutDate:dd.MM.yyyy}",
                Status = "Demo ortamında üretildi"
            });

            _logs.Insert(0, new EmailLogItem
            {
                Id = _nextId++,
                CreatedAt = DateTime.Now,
                RecipientType = "Admin",
                RecipientEmail = "admin@resortbook.local",
                Subject = "Yeni Rezervasyon Bildirimi",
                Body = $"{booking.GuestName} adlı misafir için yeni rezervasyon oluşturuldu. Oda: {roomNumber}, Tutar: {booking.TotalPrice:N0} TL",
                Status = "Demo ortamında üretildi"
            });
        }
    }

    public IReadOnlyList<EmailLogItem> GetLogs()
    {
        lock (_lock)
        {
            return _logs.ToList();
        }
    }
}

public class EmailLogItem
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string RecipientType { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}