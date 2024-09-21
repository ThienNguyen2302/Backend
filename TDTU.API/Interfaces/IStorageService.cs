namespace TDTU.API.Interfaces;

public interface IStorageService
{
	Task<List<FileDto>> Upload(List<IFormFile> files);
	Task<bool> Delete(List<string> ids);
}
