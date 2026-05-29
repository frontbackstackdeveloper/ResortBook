namespace ResortBook.ViewModels;

public class ReportViewModel
{
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public double OccupancyRate { get; set; }
    public Dictionary<string, int> BookingsByRoomType { get; set; } = new();
}
