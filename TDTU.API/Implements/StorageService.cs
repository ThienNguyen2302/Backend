using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace TDTU.API.Implements;

public class StorageService : IStorageService
{
	private readonly Cloudinary _cloudinary;
	private readonly IDataContext _context;
	public StorageService(Cloudinary cloudinary, IDataContext context)
	{
		_cloudinary = cloudinary;
		_context = context;
	}

	public async Task<bool> Delete(List<string> ids)
	{
		try
		{
			var rows = 0;
			foreach (var id in ids)
			{
				var deleteParams = new DeletionParams(id);
				var result = await _cloudinary.DestroyAsync(deleteParams);
				if (result.Result == "ok") rows++;
			}
			await DeleteMedia(ids);
			return rows > 0;
		}
		catch(Exception ex)
		{
			return false;
		}
	}

	public async Task<List<FileDto>> Upload(List<IFormFile> files)
	{
		try
		{
			if(!files.Any()) return new List<FileDto>();
			CheckFileExtension(files);
			List<FileDto> list = new List<FileDto>();
			foreach (var file in files)
			{
				var uploadResult = new ImageUploadResult();
				string PublicId = "";
				string Url = "";
				using (var stream = file.OpenReadStream())
				{
					var uploadParams = new ImageUploadParams
					{
						File = new FileDescription(file.FileName, stream)
					};
					uploadResult = await _cloudinary.UploadAsync(uploadParams);
					PublicId = uploadResult.PublicId;
					Url = uploadResult.Url.ToString();

					var item = new FileDto()
					{
						PublicId = PublicId,
						OriginalName = file.FileName,
						Extension = file.ContentType,
						Url = Url
					};
					list.Add(item);
				}
			}
			await SaveMedia(list);
			return list;
		}
		catch(Exception ex)
		{
			return new List<FileDto>();
		}
	}

	private async Task SaveMedia(List<FileDto> list)
	{
		try
		{
			if (list.Any())
			{
				List<Media> medias = new List<Media>();
				foreach (var item in list)
				{
					Media media = new Media();
					media.PublicId = item.PublicId;
					media.OriginalName = item.OriginalName;
					media.Url = item.Url;
					media.Extension = item.Extension;
					medias.Add(media);
				}
				_context.Medias.AddRange(medias);
				await _context.SaveChangesAsync();
			}
			
		}
		catch(Exception ex) { }
	}

	private async Task DeleteMedia(List<string> publicIds)
	{
		try
		{
			var medias = await _context.Medias
							   .Where(s => s.PublicId != null && publicIds.Contains(s.PublicId))
							   .ToListAsync();

			if (medias.Any())
			{
				foreach (var media in medias)
				{
					media.DeleteFlag = true;
					media.LastModifiedDate = DateTime.Now;
				}
				_context.Medias.UpdateRange(medias);
				await _context.SaveChangesAsync();
			}
		}
		catch (Exception ex) { }
	}

	private void CheckFileExtension(List<IFormFile> files)
	{
		foreach (var file in files)
		{
			var allow = IsAllowedExtension(file.ContentType);
			if (!allow) throw new ApplicationException($"File không hợp lệ: {file.ContentType}");
		}
	}

	private bool IsAllowedExtension(string extension)
	{
		switch (extension)
		{
			case ".jpg":
			case ".jpeg":
			case ".png":
			case ".gif":
			case ".jfif":

			case "image/jpg":
			case "image/jpeg":
			case "image/png":
			case "image/gif":
			case "image/jfif":

			case "application/msword":
			case "application/vnd.ms-excel":
			case "application/vnd.ms-powerpoint":
			case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
			case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
			case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
			case "application/pdf":

			case "text/csv":
			case "video/x-msvideo":
			case "video/mp4":

			case ".pdf":
			case ".xls":
			case ".xlsx":
			case ".doc":
			case ".docx":
			case ".ppt":
			case ".pptx":
				return true;
			default:
				return false;
		}
	}
}
