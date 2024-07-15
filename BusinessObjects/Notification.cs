
namespace BusinessObjects;

public class Notification : BaseEntity
{
    public DateTime? CreatedAt { get; set; }
    public string? Content { get; set; }
    public string? AccountId { get; set; }
    public int? CameraId { get; set; }
    public Account? Account { get; set; }
    public Camera? Camera { get; set; }
}