namespace TDTU.API.Models.StudentModel;

public class ImportStudentRequest
{
    public IFormFile StudentList { get; set; }
    public Guid IntershipTermId { get; set; }
    public Guid? CurrentUserId { get; set; }
}

