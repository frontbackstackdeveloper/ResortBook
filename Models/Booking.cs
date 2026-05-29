using System.ComponentModel.DataAnnotations;

namespace ResortBook.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Oda")]
    public int RoomId { get; set; }
    public Room? Room { get; set; }

    [Required(ErrorMessage = "Misafir adı zorunludur.")]
    [Display(Name = "Misafir Adı")]
    public string GuestName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
    [Display(Name = "E-posta")]
    public string GuestEmail { get; set; } = string.Empty;

    [Display(Name = "Telefon")]
    public string? GuestPhone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Giriş Tarihi")]
    public DateTime CheckInDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Çıkış Tarihi")]
    public DateTime CheckOutDate { get; set; }

    [Display(Name = "Durum")]
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    [Display(Name = "Ödeme Durumu")]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    [Display(Name = "Toplam Tutar")]
    public decimal TotalPrice { get; set; }

    [Display(Name = "Not")]
    public string? Notes { get; set; }

    [Display(Name = "Oluşturulma Tarihi")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Completed = 3
}

public enum PaymentStatus
{
    Unpaid = 0,
    Paid = 1,
    Refunded = 2
}
