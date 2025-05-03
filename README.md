# PickMe - Anket Yönetim Sistemi

## Proje Hakkında
PickMe, kullanıcıların anket oluşturmasına ve yönetmesine olanak sağlayan bir web uygulamasıdır. Kullanıcı dostu arayüzü ve güçlü özellikleri ile anketlerinizi kolayca oluşturabilir, paylaşabilir ve sonuçlarını analiz edebilirsiniz.

Proje Link : http://pickme.social/

## Özellikler
- **Anket oluşturma ve düzenleme**: Kullanıcılar, çeşitli soru türleri ile anketler oluşturabilir ve bu anketleri istedikleri zaman düzenleyebilir.
- **Anket yanıtlama**: Katılımcılar, kendilerine gönderilen anketleri kolayca yanıtlayabilirler.
- **Anket sonuçlarını görüntüleme**: Anket sahipleri, anket sonuçlarını grafikler ve istatistikler ile detaylı bir şekilde görüntüleyebilir.

## Project Structure

### Backend
- **BusinessLayer**: Servis sınıflarının bulunduğu katman.
- **DataAccessLayer**: Veritabanı işlemlerini ve veri erişimininin yönetildiği katman.
- **CoreLayer**: Class , Enum vb verilerinin tutulduğu katman.

### Frontend
- **Web**: Kullanıcı arayüzü katmanı


## Teknolojiler
- **ASP.NET Core MVC**
- **C#**
- **HTML/CSS**
- **JavaScript**
- **SQL**
- 
## Technical Stack
- .NET Core v9.0.x
- Entity Framework Core
- Clean Architecture
- N-Tier Architecture


## Kurulum
Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

1. **Depoyu klonlayın**:
    ```bash
    git clone https://github.com/fathf3/pickme.git
    ```

2. **Proje dizinine gidin**:
    ```bash
    cd pickme
    ```

3. **Gerekli bağımlılıkları yükleyin**:
    ```bash
    dotnet restore
    ```

4. **Veritabanını oluşturun ve yapılandırın**:
    - `appsettings.json` dosyasını açın ve veritabanı bağlantı ayarlarınızı yapılandırın.
    - Veritabanını oluşturmak için aşağıdaki komutu çalıştırın:
        ```bash
        dotnet ef database update
        ```

5. **Uygulamayı çalıştırın**:
    ```bash
    dotnet run
    ```

## Kullanım
Uygulama yerel sunucunuzda çalıştıktan sonra, tarayıcınızda `http://localhost:7257` adresine giderek PickMe'yi kullanabilirsiniz. 

