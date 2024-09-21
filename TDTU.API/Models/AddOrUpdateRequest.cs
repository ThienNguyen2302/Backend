namespace TDTU.API.Models;

public class AddOrUpdateRequest
{
	public DateTime? CreatedDate { get; set; }
	public DateTime? LastModifiedDate { get; set; }
	public Guid? CreatedApplicationUserId { set; get; }
	public Guid? LastModifiedApplicationUserId { set; get; }
}
