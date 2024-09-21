using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using TDTU.API.Dtos.StudentDTO;
using TDTU.API.Models.StudentModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : BaseController
	{
		private readonly IStudentService _studentService;
		public StudentController(IStudentService studentService)
		{
			_studentService = studentService;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _studentService.GetAll(request);
			var response = Result<List<StudentDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _studentService.GetById(id);
			var response = Result<StudentDto>.Success(data);
			return Ok(response);
		}
		
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] AddOrUpdateStudent request)
		{
			var data = await _studentService.Update(request);
			var response = Result<StudentDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _studentService.GetFilter(request);
			var response = Result<List<StudentDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _studentService.GetPagination(request);
			var response = Result<PaginatedList<StudentDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _studentService.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}

		[HttpPost("import")]
		public async Task<IActionResult> ImportStudent([FromForm] ImportStudentRequest request)
		{
			request.CurrentUserId = GetUserId();
            var data = await _studentService.ImportStudent(request);
            var response = Result<bool>.Success(data);
            return Ok(response);
        }

        [HttpPost("import-sample")]
        public IActionResult ImportStudentSample()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("File tải lên danh sách sinh viên mẫu");
            workSheet = ExcelHelper.GetStyle(workSheet, 2);

            workSheet.Cells[1, 1].Value = "MSSV";
            workSheet.Cells[1, 2].Value = "Họ và Tên";

            Random random = new Random();

            for (int currRow = 2; currRow <= 7; currRow++)
            {
                workSheet.Row(currRow).Height = 20;
                workSheet.Cells[currRow, 1].Value = random.Next(10000000, 100000000).ToString();
                workSheet.Cells[currRow, 2].Value = "Bui Cong Quan " + random.Next(1, 100).ToString();
            }

            workSheet.Cells.AutoFitColumns();

            return File(excel.GetAsByteArray(), "application/vnd.ms-excel", System.String.Format("{0}.xlsx", "File tải lên danh sách sinh viên mẫu"));
        }
    }
}
