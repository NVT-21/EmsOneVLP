namespace EMS.TransportWorkerService;

/// <summary>
/// Bản rút gọn cho worker: map mã sang tên hiển thị (fallback trả về mã).
/// Có thể mở rộng theo EMS.Library.XData nếu cần đầy đủ nhãn.
/// </summary>
public static class XData
{
    public static string GetNameListTransportServiceClassification(string? value) => value ?? string.Empty;

    public static string GetNameConsignment(string? value) => value ?? string.Empty;

    public static string GetNameOrderStatus(string? status) => status ?? string.Empty;

    public static string GetNameTransportPayer(string? value) => value ?? string.Empty;

    public static string GetNameCollectionType(string? value) => value ?? string.Empty;
}
