# YMH219 Key Event Logger

Windows Forms üzerinde klavye olaylarını ve olay zamanlarını gösteren C# uygulaması.

## Öğrenci

Özgür Ulusoy — 250541083

## Teknolojiler

C#, .NET 10, Windows Forms. Harici NuGet paketi kullanılmaz.

## Çalıştırma

Windows ve .NET 10 SDK gereklidir. Çözüm dosyası Visual Studio ile açılabilir veya depo klasöründe aşağıdaki komut çalıştırılabilir:

```powershell
dotnet run --project KeyEventLogger
```

## Kullanım

Başlat düğmesine basıp deneme alanına yazın. Basılan tuşlar ve saatleri listelenir. Durdur düğmesi yeni kayıt eklenmesini durdurur. Yalnızca uygulamanın kendi penceresindeki klavye olayları işlenir.

## Geliştirme durumu

İlk aşamada tuş yakalama, zaman bilgisi, Başlat/Durdur ve kayıt durumu göstergesi bulunmaktadır. Temizleme, TXT dosyasına kaydetme ve iki ekran görüntüsü henüz eklenmemiştir.

## Sınırlamalar

En son 5000 kayıt bellekte tutulur. Uygulama kapanınca kayıtlar silinir. Başka uygulamalardaki tuşlar yakalanmaz. Gizli çalışma, global klavye kancası veya ağ üzerinden gönderim bulunmaz.
