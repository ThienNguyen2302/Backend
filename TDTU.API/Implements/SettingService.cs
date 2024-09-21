using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Data;
using TDTU.API.Dtos.SettingDTO;
using TDTU.API.Dtos.SkillDTO;
using TDTU.API.Dtos.UserDTO;
using TDTU.API.Models.SettingModel;


namespace TDTU.API.Implements;

public class SettingService : ISettingService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public SettingService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<SettingDto> Get(Guid userId)
	{
		SettingDto setting = await _context.Settings.ProjectTo<SettingDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

		if(setting == null)
		{
            Setting newSetting = new Setting();
            newSetting.NumberInternshipJob = 10;
            newSetting.CreatedApplicationUserId = userId;
            newSetting.LastModifiedApplicationUserId = userId;
            newSetting.CreatedDate = DateTime.Now;
            newSetting.LastModifiedDate = DateTime.Now;

            _context.Settings.Add(newSetting);

            await _context.SaveChangesAsync();

            return _mapper.Map<SettingDto>(newSetting);
        }

		return setting;
	}

    public async Task<SettingDto> Update(UpdateSettinglRequest request)
    {
		if (request.NumberInternshipJob == null || request.NumberInternshipJob < 0) throw new Exception("Giới hạn số lượng sinh viên không hợp lệ");

		var setting = await _context.Settings.FirstOrDefaultAsync();

		setting.NumberInternshipJob = request.NumberInternshipJob;
		setting.LastModifiedApplicationUserId = request.ApplicationUserId;
		setting.LastModifiedDate = DateTime.Now;

		_context.Settings.Update(setting);
		await _context.SaveChangesAsync();

        return _mapper.Map<SettingDto>(setting);
    }
}
