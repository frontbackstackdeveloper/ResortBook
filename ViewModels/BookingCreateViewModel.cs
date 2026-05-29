using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ResortBook.ViewModels;

public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Oda seçimi zorunludur.")]
    [Display(Name = "Oda")]
    public int RoomId { get; set; }

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
    public DateTime CheckInDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Çıkış Tarihi")]
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

    [Display(Name = "Not")]
    public string? Notes { get; set; }

    public List<SelectListItem> Rooms { get; set; } = new();
}
