# ResortBook - Otel / Resort Rezervasyon Yönetim Sistemi

Bu proje Yazılım Proje Yönetimi dönem projesi için hazırlanmıştır. Proje, Freelancer üzerindeki resort booking ilanından alınan müşteri isterlerine göre ASP.NET Core MVC + SQLite + Entity Framework Core kullanılarak geliştirilir.

## Ana Özellikler

- Oda ekleme, listeleme, düzenleme ve pasif hale getirme
- Rezervasyon oluşturma
- Giriş / çıkış tarihine göre oda müsaitlik kontrolü
- Double-booking engelleme algoritması
- Rezervasyon onaylama / iptal etme
- Dashboard ve raporlama ekranları
- Ödeme ve e-posta bildirimi simülasyonu

## Algoritma Mantığı

Aynı oda için tarih çakışması şu formülle kontrol edilir:

```csharp
existing.CheckInDate < newReservation.CheckOutDate &&
newReservation.CheckInDate < existing.CheckOutDate
```

Bu koşul doğruysa sistem aynı oda için ikinci rezervasyonu engeller.

## Çalıştırma

```bash
dotnet restore
dotnet run
```

Uygulama açıldıktan sonra tarayıcıda verilen localhost adresine gidilir.

## Teslim Notu

Bu paket başlangıç kaynak kod paketidir. Sonraki aşamada kullanıcı el kitapçığı, FSM emek hesabı, SonarQube ekran çıktısı, Doxygen/Graphviz çıktıları ve sunum dosyası hazırlanacaktır.
