namespace ResortBook.ViewModels;

public class AvailabilityViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<DateTime> Dates { get; set; } = new();
    public List<AvailabilityRoomViewModel> Rooms { get; set; } = new();
}

public class AvailabilityRoomViewModel
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public List<AvailabilityDayViewModel> Days { get; set; } = new();
}

public class AvailabilityDayViewModel
{
    public DateTime Date { get; set; }
    public bool IsAvailable { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string? GuestName { get; set; }
    public int? BookingId { get; set; }
}