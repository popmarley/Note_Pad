# Note_Pad

Windows için geliştirilmiş, hızlı ve kullanışlı bir metin düzenleme uygulamasıdır. Temel not defteri işlevlerinin yanında gelişmiş biçimlendirme, metin karşılaştırma, dikte, dışa aktarma ve güvenli dosya yönetimi özellikleri içerir.

## Özellikler

- Dosya açma, kaydetme, farklı kaydetme ve dosya adını değiştirme
- UTF-8, UTF-8 BOM ve UTF-16 dosya okuma desteği
- Son dosyalar menüsü
- Otomatik kurtarma desteği
- Bul ve değiştir
- Yazı tipi, boyut, kalın, italik, altı çizili, hizalama ve renk seçenekleri
- Satır numaraları, zoom ve durum çubuğu
- Açık/koyu tema
- Dikte
- Metin karşılaştırıcı
- PDF, Word, Excel, PowerPoint, HTML, JPG ve PNG dışa aktarma
- Güvenli güncelleme kontrolü

## Son Sürüm

Versiyon 3.6 (is Derlemesi 1047)

Bu sürümde dosya encoding algılama, güvenli kaydetme, otomatik kurtarma, son dosyalar, dosya adı değiştirme, yazı/arka plan rengi, daha güvenli güncelleme kontrolü ve performans iyileştirmeleri eklendi.

## Derleme

Proje .NET Framework 4.7.2 ve Windows Forms kullanır. Visual Studio 2022 ile açıp `Not_Defteri.sln` üzerinden derleyebilirsiniz.

Komut satırından derlemek için Visual Studio MSBuild kullanılmalıdır:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" ".\Note_Pad-master\Note_Pad-master\Not_Defteri.sln" /p:Configuration=Debug /p:Platform="Any CPU"
```

## Not

`dotnet msbuild` eski .NET Framework WinForms kaynak üretiminde takılabilir. Bu nedenle bu proje için Visual Studio MSBuild önerilir.
