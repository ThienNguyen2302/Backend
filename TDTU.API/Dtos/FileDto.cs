using AutoMapper;

namespace TDTU.API.Dtos;

public class FileDto
{
	public string PublicId { get; set; }
	public string OriginalName { get; set; }
	public string Extension { get; set; }
	public string Url { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<FileDto, Media>();
		}
	}
}
