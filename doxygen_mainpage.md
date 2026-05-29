# ResortBook Kod Dokümantasyonu

## Proje Özeti

ResortBook, Freelancer üzerinde yer alan resort/otel rezervasyon sistemi isterleri temel alınarak geliştirilen bir ASP.NET Core MVC uygulamasıdır.

Uygulama; oda yönetimi, rezervasyon oluşturma, rezervasyon onaylama, ödeme durumu takibi, gelir hesaplama ve raporlama işlevlerini içerir.

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQLite
- Razor Views
- HTML / CSS
- Doxygen
- Graphviz

## Temel Modüller

- DashboardController: Genel özet, gelir ve son rezervasyon bilgilerini gösterir.
- RoomsController: Oda ekleme, düzenleme, detay ve listeleme işlemlerini yönetir.
- BookingsController: Rezervasyon oluşturma, onaylama ve iptal işlemlerini yönetir.
- ReportsController: Gelir, doluluk oranı ve oda tipine göre rezervasyon raporlarını üretir.
- BookingService: Tarih çakışması ve toplam fiyat hesaplama gibi iş kurallarını içerir.

## Ana İş Kuralı: Double-Booking Engelleme

Sistem, aynı odanın aynı tarih aralığında ikinci kez rezerve edilmesini engeller.

Tarih çakışması kontrolü şu mantıkla yapılır:

existing.CheckInDate < newReservation.CheckOutDate

ve

newReservation.CheckInDate < existing.CheckOutDate

Bu koşul sağlanırsa sistem rezervasyonu kaydetmez ve kullanıcıya uyarı verir.

## Gelir Hesaplama Mantığı

Gelir hesabına yalnızca durumu Onaylandı ve ödeme durumu Ödendi olan rezervasyonlar dahil edilir.

Beklemede veya ödenmemiş rezervasyonlar toplam gelire dahil edilmez.
