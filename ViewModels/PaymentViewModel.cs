using System.ComponentModel.DataAnnotations;

namespace ResortBook.ViewModels;

public class PaymentViewModel
{
    public int BookingId { get; set; }

    public string GuestName { get; set; } = string.Empty;

    public string RoomInfo { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Kart üzerindeki ad soyad zorunludur.")]
    [Display(Name = "Kart Üzerindeki Ad Soyad")]
    public string CardHolderName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kart numarası zorunludur.")]
    [MinLength(16, ErrorMessage = "Kart numarası en az 16 karakter olmalıdır.")]
    [MaxLength(19, ErrorMessage = "Kart numarası en fazla 19 karakter olabilir.")]
    [Display(Name = "Kart Numarası")]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Son kullanma tarihi zorunludur.")]
    [Display(Name = "Son Kullanma Tarihi")]
    public string ExpiryDate { get; set; } = string.Empty;

    [Required(ErrorMessage = "CVV zorunludur.")]
    [MinLength(3, ErrorMessage = "CVV en az 3 karakter olmalıdır.")]
    [MaxLength(4, ErrorMessage = "CVV en fazla 4 karakter olabilir.")]
    [Display(Name = "CVV")]
    public string Cvv { get; set; } = string.Empty;
}