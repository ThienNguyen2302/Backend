using TDTU.API.Dtos.SettingDTO;
using TDTU.API.Models.SettingModel;

namespace TDTU.API.Interfaces;

public interface ISettingService
{
	Task<SettingDto> Get(Guid userId);
    Task<SettingDto> Update(UpdateSettinglRequest request);
}
