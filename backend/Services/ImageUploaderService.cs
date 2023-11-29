namespace lunarwatch.backend.Services;

public class ImageUploaderService
{
  public async Task<string> UploadFileToStaticFiles(IFormFile file)
  {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    FileInfo fileInfo = new FileInfo(file.FileName);
    string fileName = file.FileName;
    string fileNameWithPath = Path.Combine(path, fileName);
    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    return fileNameWithPath.Split("wwwroot")[1];
  }
}