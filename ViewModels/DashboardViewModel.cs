using ResortBook.Models;

namespace ResortBook.ViewModels;

public class DashboardViewModel
{
    public int TotalRooms { get; set; }
    public int ActiveRooms { get; set; }
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public double OccupancyRate { get; set; }
    public List<Booking> LatestBookings { get; set; } = new();
}
