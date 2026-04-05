using Alumni_Portal.FileUploads.DTO;

namespace Alumni_Portal.FileUploads
{
    public class FileService
    {
        private readonly string _mediaRootPath;

        private static readonly string[] AllowedImageTypes = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private static readonly string[] AllowedVideoTypes = { ".mp4", ".mov", ".avi", ".mkv", ".webm" };
        private static readonly string[] AllowedExtensions = AllowedImageTypes.Concat(AllowedVideoTypes).ToArray();

        private const long MaxImageSize = 5 * 1024 * 1024;
        private const long MaxVideoSize = 100 * 1024 * 1024;
        private const int MaxFileCount = 10;

        public FileService(IWebHostEnvironment env)
        {
            var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
            _mediaRootPath = Path.Combine(webRoot, "uploads");
            
        }

        public async Task<UploadResponseDTO> UploadMedia(List<IFormFile> media)
        {
            var response = new UploadResponseDTO();

            if (media == null || !media.Any())
            {
                response.errorMessage = "No media provided.";
                return response;
            }

            if (media.Count > MaxFileCount)
            {
                response.errorMessage = $"Maximum {MaxFileCount} files allowed.";
                return response;
            }

            foreach (var file in media)
            {
                if (file.Length == 0)
                {
                    response.errors.Add($"{file.FileName}: File is empty.");
                    continue;
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    response.errors.Add($"{file.FileName}: File type '{extension}' is not allowed.");
                    continue;
                }

                var isVideo = AllowedVideoTypes.Contains(extension);
                var maxSize = isVideo ? MaxVideoSize : MaxImageSize;
                if (file.Length > maxSize)
                {
                    response.errors.Add($"{file.FileName}: Exceeds {maxSize / (1024 * 1024)}MB limit.");
                    continue;
                }

                if (!await IsValidFileSignatureAsync(file, isVideo))
                {
                    response.errors.Add($"{file.FileName}: File content does not match its extension.");
                    continue;
                }

                var safeFileName = Path.GetFileName(file.FileName);
                var fileName = $"{Guid.NewGuid()}_{safeFileName}";
                var filePath = Path.Combine(_mediaRootPath, fileName);

                try
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    response.UploadedFiles.Add(new FileUploadDTO
                    {
                        Media_Date = DateTime.UtcNow,
                        Media_Title = Path.GetFileNameWithoutExtension(file.FileName),
                        Media_File_Name = fileName,
                        Media_File_Location = $"/uploads/{fileName}",
                    });
                }
                catch (Exception ex)
                {
                    if (File.Exists(filePath)) File.Delete(filePath);
                    response.errors.Add($"{file.FileName}: Failed to save. {ex.Message}");
                }
            }

            return response;
        }

        private static async Task<bool> IsValidFileSignatureAsync(IFormFile file, bool isVideo)
        {
            await using var stream = file.OpenReadStream();
            var header = new byte[8];
            await stream.ReadAsync(header, 0, header.Length);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => header[0] == 0xFF && header[1] == 0xD8,
                ".png" => header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47,
                ".gif" => header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46,
                ".webp" => header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46,
                ".mp4" or ".mov" => header[4] == 0x66 && header[5] == 0x74 && header[6] == 0x79 && header[7] == 0x70,
                ".avi" => header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46,
                ".webm" => header[0] == 0x1A && header[1] == 0x45 && header[2] == 0xDF && header[3] == 0xA3,
                _ => false
            };
        }
    }
}