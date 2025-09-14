<!-- Restaurant API / Masalar & Sipariş Takibi README.md -->

<div align="center">

![Restaurant System Architecture](https://via.placeholder.com/800x300.png?text=Architecture+Diagram)  

**Restaurant API & Real-Time Order Tracking System**

</div>

---

## 🚀 Proje Açıklaması

Bu proje, restoranlarda **mobil cihaz üzerinden sipariş alımı**, **masaların ve yiyecek/iceceklerin takibi**, **anlık durum senkronizasyonu** gibi işlevleri sağlayan bir sistemdir. API katmanı, WebSocket ve REST API kullanarak Flutter uygulamaları ile MSSQL veritabanı arasında iletişim sağlar. Böylece garson ya da servis elemanı, siparişleri masalardan tekil cihazlarla kontrol edip, fiş yazdırma ya da otomasyon yazılımlarına çok fazla ihtiyaç duymadan işleri yürütür.

---

## 🛠️ Teknolojiler & Altyapı

| Katman | Teknoloji / Kullanım |
|---|---|
| Backend | **.NET (REST API + WebSocket)** — sipariş verisi, masa durumu, kullanıcı doğrulama |
| Veritabanı | **MSSQL** — masa, sipariş, menü, kullanıcı, oturum, kategori verileri |
| Mobil Uygulama | **Flutter** — kullanıcı arayüzü, gerçek zamanlı durum güncellemesi |
| Lokal Depolama | (varsa) offline cahing / geçici veri saklama |
| Güvenlik | Authentication & Authorization (örneğin JWT / .NET Identity) |
| Menü Yapısı | Yemekler / içecekler kategorilerine göre ayrılıyor, fiyatlandırma dahil |
| Senkronizasyon | WebSocket üzerinden anlık bildirim, durum takibi |

---

## 🔑 Özellikler

- 👤 **Authentication & Authorization** — kullanıcı logini, roller, yetki kontrolleri  
- 🗄️ **Masa Yönetimi** — masaların doluluk durumu, açık sipariş, yani masa bazlı takip  
- 🥘 **Menü Kategorileri ve Fiyatlandırma** — içecek / yemek kategorileri, menü güncelleme  
- 📡 **Gerçek Zamanlı Sipariş Takibi** — WebSocket ile her sipariş anlık diğer bağlı cihazlara / dashboard’a iletilir  
- 🔒 **Veritabanı Senkronizasyonu** — MSSQL üzerinden tüm cihazlarda güncel veri  
- 📊 **Yiyecek / İçecek Hazırlama Takibi** — mutfak kısmı, içecek kısmı gibi bölümlerde siparişin hazırlanma durumu takip edilir  

---

## 🔗 API & Endpoint Örnekleri

| Endpoint | Method | Açıklama |
|---|---|---|
| `/api/auth/register` | POST | Yeni kullanıcı kaydı |
| `/api/auth/login` | POST | Kullanıcı girişi & JWT token alınması |
| `/api/tables` | GET | Masaların durumu listesini al |
| `/api/menus` | GET | Menü öğelerini kategoriye göre getir |
| `/api/orders` | POST | Yeni sipariş oluştur |
| `/api/orders/{id}` | GET | Sipariş detaylarını al |
| WebSocket URL | ‐ | Sipariş durum değişiklikleri ve masa durum güncellemeleri için anlık bağlantı |

---

## 🔍 Mimari & Workflow

1. Mobil uygulama kullanıcı giriş bilgileriyle API’ye bağlanır → token alır.  
2. Menü ve kategori verisi REST API üzerinden çekilir; bu veriler Flutter tarafında düzgün şekilde listelenir.  
3. Garson cihazı üzerinden masa ve sipariş bilgisi girilir → sipariş API’ye POST edilir.  
4. Backend, veritabanına yazarken WebSocket ile tüm bağlı cihazlara (örneğin mutfak ekranı, diğer garson cihazı) siparişin durumu gönderilir.  
5. Menü öğesi seçimi, hazır olma durumu gibi değişiklikler yine WebSocket’le “publish/subscribe” tarzında yayılır.  
6. MSSQL veritabanında tüm siparişler, masa durumları, kullanıcı rolleri etc. güvenli şekilde saklanır.  

---

## 📦 Kurulum & Çalıştırma

### Backend (.NET)

```bash
# Depoyu klonla
git clone https://github.com/kullaniciAdin/restaurant-api.git
cd restaurant-api

# Bağımlılıkları yükle
dotnet restore

# Güvenlik ayarlarını yapılandır
# - JWT secret
# - Veritabanı bağlantısı (connection string)
# - WebSocket endpoint konfigürasyonu

# Veritabanı migrate et
dotnet ef database update

# Uygulamayı başlat
dotnet run
```

```bash
# Mobil repo
git clone https://github.com/kullaniciAdin/restaurant-flutter.git
cd restaurant-flutter

# Paketleri yükle
flutter pub get

# Uygulamayı çalıştır
flutter run
```

🎯 Gelecek Planları

📱 Dashboard — yönetici paneli, mutfak ekranı vs görsel yönüyle detaylı raporlar

🧮 Stok Takibi — malzeme giriş çıkış, kritik stok seviyesi uyarıları

🔁 Multi-şube desteği — farklı restoranlar / farklı adresler arasında senkronizasyon

🔔 Bildirimler — sipariş hazır olduğunda sesli/ görsel bildirimler

🌐 API dokümantasyonu (Swagger / OpenAPI)

📬 İletişim: yusufdgrl72@gmail.com

🌐 Web: yusufdegerli.github.io
