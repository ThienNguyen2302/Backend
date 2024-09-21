using TDTU.API.Dtos.StudentDTO;
using TDTU.API.Models.StudentModel;

namespace TDTU.API.Interfaces;

public interface IStudentService
{
	Task<PaginatedList<StudentDto>> GetPagination(PaginationRequest request);
	Task<List<StudentDto>> GetAll(BaseRequest request);
	Task<List<StudentDto>> GetFilter(FilterRequest request);
	Task<StudentDto> GetById(Guid id);
	Task<StudentDto> Update(AddOrUpdateStudent request);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<bool> ImportStudent(ImportStudentRequest request);
}
