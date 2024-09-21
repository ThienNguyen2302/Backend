namespace TDTU.API.Models;

public class DeleteRequest
{
	public List<string> Ids { set; get; }
	public Guid ApplicationUserId { get; set; }
}
