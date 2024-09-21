using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.SettingDTO;
using TDTU.API.Models.SettingModel;

namespace TDTU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _settingService.Get(GetUserId() ?? Guid.Empty);
            var response = Result<SettingDto>.Success(data);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateSettinglRequest request)
        {
            var data = await _settingService.Update(request);
            var response = Result<SettingDto>.Success(data);
            return Ok(response);
        }
    }
}
