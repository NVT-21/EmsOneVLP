# Kế hoạch & Hướng dẫn host `EMS.EomsCategorySyncWorker` lên Windows Service

## 1. Khả năng tương thích sẵn có của Project
Code của worker đã được tích hợp sẵn package:
- `Microsoft.Extensions.Hosting.WindowsServices` (v9.0.2)
- Lệnh đăng ký trong `Program.cs`:
  ```csharp
  builder.Services.AddWindowsService(options =>
  {
      options.ServiceName = "EMS.EomsCategorySyncWorker";
  });
  ```
- Cơ chế này tự động nhận diện:
  - Nếu chạy từ command line (`dotnet run` hoặc double-click `.exe`): Chạy ở chế độ Console bình thường (tiện test/debug).
  - Nếu khởi động bởi Windows Service Control Manager: Tự động chạy nền dưới dạng **Windows Service**, tự động chuyển thư mục làm việc về thư mục chứa file `.exe` (để đọc đúng `appsettings.json`), và tự động tích hợp ghi log vào **Windows Event Viewer**.

---

## 2. Quy trình triển khai từng bước lên Windows Server

```mermaid
flowchart TD
    A["1. dotnet publish Release"] --> B["2. Copy thư mục publish lên Server"]
    B --> C["3. Cấu hình appsettings.json trên Server"]
    C --> D["4. Đăng ký Service qua sc.exe / PowerShell"]
    D --> E["5. Cấu hình Tự phục hồi (Recovery / Auto-Restart)"]
    E --> F["6. Khởi động Service & Kiểm tra Event Viewer / Mongo"]
```

---

### Bước 1: Build & Publish bản chạy cho Windows (Publish)

Mở PowerShell tại máy dev và chạy lệnh publish:

#### Lựa chọn A (Khuyên dùng - Self-contained: Server KHÔNG CẦN cài .NET 9):
Toàn bộ runtime .NET 9 sẽ được đóng gói kèm theo, server Windows chỉ cần nhận thư mục là chạy ngay:
```powershell
cd D:\EMONE\EmsOneVLP\13.EMS.EomsCategorySyncWorker\EMS.EomsCategorySyncWorker
dotnet publish -c Release -r win-x64 --self-contained true -o D:\Deploy\EMS.EomsCategorySyncWorker
```

#### Lựa chọn B (Framework-dependent - Nhẹ hơn, nhưng Server PHẢI CÓ .NET 9 Runtime):
```powershell
cd D:\EMONE\EmsOneVLP\13.EMS.EomsCategorySyncWorker\EMS.EomsCategorySyncWorker
dotnet publish -c Release -r win-x64 --self-contained false -o D:\Deploy\EMS.EomsCategorySyncWorker
```

---

### Bước 2: Đưa thư mục lên Server và Cấu hình

1. Copy thư mục `EMS.EomsCategorySyncWorker` vừa publish lên Windows Server (ví dụ đặt tại đường dẫn: `C:\Services\EMS.EomsCategorySyncWorker` hoặc `D:\Services\EMS.EomsCategorySyncWorker`).
2. Mở file `appsettings.json` trong thư mục trên server và điều chỉnh thông số môi trường:
   - `ConnectionStrings:EMSONE`: Chuỗi kết nối Oracle của môi trường server.
   - `EomsCategory:BaseUrl`: URL API EOMS (`https://uat-vlp-edi.vnpost.vn` hoặc Prod).
   - `EomsCategory:SecretKey`: Secret key (`9CnQHmC8qUDhbQ7VTbU4`).
   - `EomsCategory:MongoConnectionString`: Chuỗi kết nối MongoDB (`mongodb://admin:ems14052026@192.168.86.16:27017/`).
   - `EomsCategory:RunAtLocalTime`: `00:00`
   - `EomsCategory:RunOnStartup`: Để `false` (nếu muốn nạp ngay 1 lần lúc vừa cài thì để `true`, sau đó đổi lại `false`).

---

### Bước 3: Đăng ký Windows Service

Mở **Command Prompt (CMD)** hoặc **PowerShell** với quyền **Administrator** (`Run as Administrator`):

#### 1. Lệnh tạo Service:
> [!IMPORTANT]
> Lưu ý cú pháp của lệnh `sc.exe`: dấu cách ngay sau `binPath=` và `start=` là bắt buộc theo quy định của Windows.

```cmd
sc.exe create "EMS.EomsCategorySyncWorker" binPath= "C:\Services\EMS.EomsCategorySyncWorker\EMS.EomsCategorySyncWorker.exe" start= auto
```

#### 2. Đặt mô tả cho Service (Description):
```cmd
sc.exe description "EMS.EomsCategorySyncWorker" "Dich vu dong bo danh muc Tinh/Xa/San pham EOMS vao Oracle va MongoDB luc 00:00 hang ngay."
```

#### 3. Cấu hình tự khởi động lại khi có sự cố (Recovery / Auto-Restart):
Cấu hình service tự restart lại sau 60 giây nếu bị dừng đột ngột:
```cmd
sc.exe failure "EMS.EomsCategorySyncWorker" reset= 86400 actions= restart/60000/restart/60000/restart/60000
```

---

### Bước 4: Khởi động và Kiểm tra trạng thái

#### 1. Khởi động service:
```cmd
sc.exe start "EMS.EomsCategorySyncWorker"
```
*(Hoặc gõ `services.msc` trong hộp thoại Run `Win + R`, tìm service **EMS.EomsCategorySyncWorker** và bấm **Start**).*

#### 2. Kiểm tra log vận hành:
- **Cách 1 - Windows Event Viewer:**
  1. Nhấn `Win + R`, gõ `eventvwr.msc`.
  2. Vào `Windows Logs` -> `Application`.
  3. Bạn sẽ thấy các log Information / Error từ nguồn `EMS.EomsCategorySyncWorker`.
- **Cách 2 - MongoDB:**
  Kiểm tra collection `EomsCategorySyncLog` trên MongoDB `emsoneloguat` để xem log phiên chạy.

---

### Bước 5: Cập nhật bản mới (Update Release) hoặc Gỡ bỏ (Uninstall)

#### Khi cập nhật code mới:
1. Dừng service:
   ```cmd
   sc.exe stop "EMS.EomsCategorySyncWorker"
   ```
2. Copy các file `.dll`, `.exe` mới đè vào thư mục `C:\Services\EMS.EomsCategorySyncWorker` (giữ nguyên file `appsettings.json`).
3. Khởi động lại service:
   ```cmd
   sc.exe start "EMS.EomsCategorySyncWorker"
   ```

#### Khi muốn gỡ bỏ hoàn toàn Service:
```cmd
sc.exe stop "EMS.EomsCategorySyncWorker"
sc.exe delete "EMS.EomsCategorySyncWorker"
```

---

## 3. Đề xuất: Tạo sẵn Script tự động (.ps1) trong Project
Để tiện lợi cho bạn hoặc đội vận hành triển khai trên server Windows chỉ bằng 1 cú click chuột, tôi có thể tạo sẵn:
1. `publish-windows.bat`: Tự động publish ra thư mục deploy.
2. `install-service.ps1`: Tự động đăng ký service + cấu hình recovery + start service.
3. `uninstall-service.ps1`: Tự động stop + gỡ service.

Bạn có muốn tôi tạo các file script tự động này vào thư mục `13.EMS.EomsCategorySyncWorker` không?
