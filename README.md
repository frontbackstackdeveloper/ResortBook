# ResortBook

ResortBook, Yazılım Proje Yönetimi dersi kapsamında geliştirilen bir otel ve resort rezervasyon yönetim sistemidir.

## Proje Amacı

Bu proje, Freelancer üzerinde yer alan resort booking ilanındaki temel müşteri isterleri dikkate alınarak geliştirilmiştir.

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- C#
- Razor Views
- Bootstrap
- SonarCloud
- Doxygen
- Graphviz

## Temel Özellikler

- Dashboard ekranı
- Oda yönetimi
- Rezervasyon oluşturma
- Rezervasyon listeleme
- Rezervasyon onaylama
- Rezervasyon iptal etme
- Ödeme durumu takibi
- Raporlama ekranı
- Double-booking engelleme algoritması
- Gelir hesaplama mantığı

## Double-Booking Kontrolü

Sistem, aynı oda için çakışan tarih aralıklarında ikinci rezervasyon oluşturulmasını engeller.

Bu kontrol sayesinde aynı oda aynı tarih aralığında iki farklı müşteriye atanamaz.

## Gelir Hesaplama

Gelir hesaplamasında yalnızca Onaylandı ve Ödendi durumundaki rezervasyonlar dikkate alınır.

Beklemede veya ödenmemiş rezervasyonlar toplam gelire dahil edilmez.

## Proje Yönetimi Çıktıları

Projede task board, kullanıcı el kitapçığı, FSM emek hesabı, Doxygen çıktısı, Graphviz diyagramı ve SonarCloud kalite analizi hazırlanmıştır.
