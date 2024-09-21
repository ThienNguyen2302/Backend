using System.ComponentModel.DataAnnotations;

namespace TDTU.API.Data;

public class BaseEntity
{
	public BaseEntity()
	{
		Id = Guid.NewGuid();
		DeleteFlag = false;
		CreatedDate = System.DateTime.Now;
		LastModifiedDate = System.DateTime.Now;
	}

	[Key]
	public Guid Id { set; get; }
	public bool DeleteFlag { set; get; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? LastModifiedDate { get; set; }
	public Guid? CreatedApplicationUserId { set; get; }
	public Guid? LastModifiedApplicationUserId { set; get; }
}

public class BaseStatusEntity
{
	public BaseStatusEntity()
	{
		Id = StringHelper.GenerateCode();
		DeleteFlag = false;
		CreatedDate = System.DateTime.Now;
		LastModifiedDate = System.DateTime.Now;
	}

	[Key]
	public string Id { set; get; }
	public bool DeleteFlag { set; get; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? LastModifiedDate { get; set; }
	public Guid? CreatedApplicationUserId { set; get; }
	public Guid? LastModifiedApplicationUserId { set; get; }
}