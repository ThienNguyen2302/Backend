namespace TDTU.API.Models.InternshipOrderModel;

public class InternshipOderUpdateStatus : AddOrUpdateRequest
{
    public Guid InternshipOderId { get; set; }
    public string Status { get; set; }
}
