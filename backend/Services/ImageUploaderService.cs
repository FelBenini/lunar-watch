namespace lunarwatch.backend.Services;

public class ImageUploaderService
{
  public async Task<string> UploadFileToStaticFiles(IFormFile file)
  {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    FileInfo fileInfo = new FileInfo(file.FileName);
    string fileName = file.FileName;
    string fileNameSplit = fileName.Split(fileInfo.Extension)[0];
    long currentDate = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
    string fileNameWithPath = Path.Combine(path, fileNameSplit + currentDate + fileInfo.Extension);
    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    return fileNameWithPath.Split("wwwroot")[1];
  }
}