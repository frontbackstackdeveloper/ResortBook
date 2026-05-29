using System.ComponentModel.DataAnnotations;

namespace ResortBook.Models;

public class Room
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Oda numarası zorunludur.")]
    [Display(Name = "Oda Numarası")]
    public string RoomNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Oda tipi zorunludur.")]
    [Display(Name = "Oda Tipi")]
    public string RoomType { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "Kapasite 1 ile 20 arasında olmalıdır.")]
    [Display(Name = "Kapasite")]
    public int Capacity { get; set; }

    [Range(1, 100000, ErrorMessage = "Gecelik fiyat geçerli olmalıdır.")]
    [Display(Name = "Gecelik Fiyat")]
    public decimal PricePerNight { get; set; }

    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
