# Luồng chạy `EMS.EomsCategorySyncWorker`

Worker độc lập (.NET 9) đồng bộ 3 danh mục EOMS (`EOMS_PROVINCES`, `EOMS_COMMUNES`, `EOMS_PRODUCT`) từ API `POST {BaseUrl}/api/unified` vào Oracle qua package `PKG_EOMS_CATEGORY`, và ghi log kết quả/lỗi vào MongoDB `EomsCategorySyncLog`.

## 1. Thành phần chính

- `Program.cs`: đăng ký `HttpClient("EomsCategory")`, `EomsCategoryOptions`, `EomsCategoryClient`, `EomsCategoryMongoRepository`, `EomsCategoryRepository`, và hosted service `EomsCategorySyncWorker`. Hỗ trợ chạy Docker (Linux) hoặc Windows Service (`AddWindowsService`).
- `Workers/EomsCategorySyncWorker.cs`: lập lịch chạy hàng ngày lúc `RunAtLocalTime` (`00:00` múi giờ `Asia/Ho_Chi_Minh`), hỗ trợ `RunOnStartup`, dùng `SemaphoreSlim` chống chạy chồng.
- `Services/EomsCategoryClient.cs`: gọi `POST /api/unified` kèm header `sign` (MD5 từ `EomsSignatureHelper`), có retry backoff khi lỗi mạng/5xx.
- `Repositories/EomsCategoryRepository.cs`: preload `(CODE, RAW_JSON)` từ Oracle để đếm chính xác `InsertedCount`, `UpdatedCount`, `UnchangedCount`, bỏ qua bản ghi không đổi, chỉ gọi `PKG_EOMS_CATEGORY.UPSERT_*` khi có thay đổi (không bao giờ `DELETE`/`TRUNCATE`). Dừng sớm khi gặp `ORA-06550`.
- `Repositories/EomsCategoryMongoRepository.cs`: ghi log chi tiết từng batch (`EOMS_PROVINCES`, `EOMS_COMMUNES`, `EOMS_PRODUCT`), lỗi API/DB và bản ghi `SUMMARY` vào MongoDB.

## 2. Triển khai bằng Docker

```bash
cd 13.EMS.EomsCategorySyncWorker/EMS.EomsCategorySyncWorker
cp .env.example .env
# Sửa ConnectionStrings__EMSONE và EomsCategory__* trong .env
docker compose up -d --build
```

Xem log container:
```bash
docker logs -f ems-eoms-category-sync-worker
```
