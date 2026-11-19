# Kế hoạch & Hướng dẫn tự Host thử nghiệm ngay trên máy của bạn

Máy tính hiện tại của bạn đã kết nối thông suốt tới cả **Oracle (`192.168.80.60:1521` - OK)** và **MongoDB (`192.168.86.16:27017` - OK)**.

Bạn có thể chọn 1 trong 2 cách sau để chạy thử:

---

## Cách 1: Chạy trực tiếp ở chế độ Console (Khuyên dùng để test - Xem log ngay lập tức)

Vì worker được viết bằng .NET Worker Service, khi bạn chạy file `.exe` trực tiếp từ dòng lệnh, nó sẽ chạy như một **Console App bình thường**, log sẽ in trực tiếp ra màn hình để bạn theo dõi.

### Các bước thực hiện:
1. Mở cửa sổ **PowerShell** hoặc **CMD**.
2. Di chuyển vào thư mục đã publish:
   ```powershell
   cd D:\EMONE\EmsOneVLP\13.EMS.EomsCategorySyncWorker\deploy_package
   ```
3. Chạy file exe:
   ```powershell
   .\EMS.EomsCategorySyncWorker.exe
   ```
4. **Hiện tượng quan sát được:**
   - Trong `deploy_package\appsettings.json`, cờ `"RunOnStartup": true` đang bật.
   - Ứng dụng sẽ chạy ngay 1 vòng đồng bộ:
     - Lấy 63 tỉnh/thành -> gọi Oracle package `PKG_EOMS_CATEGORY.UPSERT_PROVINCES`.
     - Lấy danh sách xã theo từng tỉnh -> gọi Oracle package `PKG_EOMS_CATEGORY.UPSERT_COMMUNES`.
     - Lấy danh sách sản phẩm -> gọi Oracle package `PKG_EOMS_CATEGORY.UPSERT_PRODUCT`.
     - Ghi log kết quả chi tiết từng phần và log `SUMMARY` vào MongoDB.
   - Bạn sẽ thấy log in từng dòng trên màn hình Console.
5. **Dừng chạy:** Nhấn `Ctrl + C` để tắt.

---

## Cách 2: Cài đặt và chạy thật dạng Windows Service (Giống 100% trên Server)

Cách này sẽ đăng ký worker vào hệ thống Windows của máy bạn giống hệt như khi đưa lên Server thật.

### Bước 1: Cài đặt Service
1. Mở File Explorer, vào thư mục:
   `D:\EMONE\EmsOneVLP\13.EMS.EomsCategorySyncWorker\deploy_package`
2. Tìm file **`install-service.ps1`**.
3. **Click chuột phải** vào file `install-service.ps1` -> Chọn **Run with PowerShell** (bấm *Yes* nếu hộp thoại UAC hỏi cấp quyền Administrator).
4. Script sẽ tự động:
   - Đăng ký service `EMS.EomsCategorySyncWorker` vào Windows.
   - Cấu hình tự động bật cùng Windows (`start=auto`).
   - Cấu hình tự restart khi gặp sự cố (`sc failure ...`).
   - Khởi động service luôn (`Start-Service`).

---

### Bước 2: Kiểm tra Service đang chạy
1. Nhấn tổ hợp phím `Win + R`, gõ:
   ```text
   services.msc
   ```
2. Tìm service có tên: **EMS EOMS Category Sync Worker** (hoặc `EMS.EomsCategorySyncWorker`).
3. Kiểm tra cột **Status** hiển thị: **`Running`**.

---

### Bước 3: Xem Log hoạt động
1. **Qua Windows Event Viewer:**
   - Nhấn `Win + R`, gõ `eventvwr.msc`.
   - Vào `Windows Logs` -> `Application`.
   - Bạn sẽ thấy các log Information từ nguồn `EMS.EomsCategorySyncWorker`.
2. **Qua MongoDB Compass:**
   - Mở MongoDB Compass -> Kết nối `mongodb://admin:ems14052026@192.168.86.16:27017/`.
   - Vào database `emsoneloguat` -> collection `EomsCategorySyncLog`.
   - Tìm `{ Category: "SUMMARY" }` để xem bản ghi kết quả lần chạy.

---

### Bước 4: Gỡ bỏ Service sau khi test xong (Rất sạch sẽ)
Khi bạn test xong và không muốn để nó chạy ngầm trên máy nữa:
1. Vào lại thư mục `D:\EMONE\EmsOneVLP\13.EMS.EomsCategorySyncWorker\deploy_package`.
2. **Click chuột phải** vào file **`uninstall-service.ps1`** -> Chọn **Run with PowerShell**.
3. Service sẽ được dừng và xóa hoàn toàn khỏi máy tính của bạn trong vài giây.

---

## 3. Bạn muốn thực hiện cách nào?
- Bạn có muốn tự thử mở PowerShell gõ `.\EMS.EomsCategorySyncWorker.exe` (Cách 1) để nhìn thấy log chạy ngay bây giờ không?
- Hoặc bạn muốn tôi chạy thử Cách 1 ngay trong phiên làm việc này để xem kết quả đồng bộ thực tế?
