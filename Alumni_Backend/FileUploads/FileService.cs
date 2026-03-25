//using Entity_Directories.Services.DTO;
//using Microsoft.AspNetCore.Mvc;
//using Alumni_Portal.FileUploads.DTO;
//using System.ComponentModel;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//namespace Alumni_Portal.FileUploads
//{
//    public class FileService 
//    {
//        private readonly string _mediaRootPath;

//        // Allowed file types
//        private static readonly string[] AllowedImageTypes = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
//        private static readonly string[] AllowedVideoTypes = { ".mp4", ".mov", ".avi", ".mkv", ".webm" };
//        private static readonly string[] AllowedExtensions = AllowedImageTypes.Concat(AllowedVideoTypes).ToArray();

//        // Size limits
//        private const long MaxImageSize = 5 * 1024 * 1024;   // 5 MB
//        private const long MaxVideoSize = 100 * 1024 * 1024; // 100 MB
//        private const int MaxFileCount = 10;

//        public FileService(IWebHostEnvironment env)
//        {
//            _mediaRootPath = Path.Combine(env.WebRootPath, "uploads");
            
//        }

        
//        public async  Task<UploadResponseDTO>   UploadMedia(List<IFormFile> Media)
//        {
//            var response = new UploadResponseDTO
//            {
//                UploadedFiles = new List<FileUploadDTO>(),
//                errorMessage = string.Empty
//            };
//            if (Media == null || !Media.Any())
//                response.errorMessage = "No media is uploaded";
//                return response;


//                if (Media.Count > MaxFileCount)
//                    response.errorMessage=$"You can upload a maximum of {MaxFileCount} files at once.";
//                    return response;

//            var errors = new List<string>();
//            var mediaEntities = new List<FileUploadDTO>();

//            foreach (var file in Media)
//            {
//                // 3. Empty file check
//                if (file.Length == 0)
//                {
//                    errors.Add($"{file.FileName}: File is empty.");
//                    continue;
//                }

//                // 4. Extension check
//                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
//                if (!AllowedExtensions.Contains(extension))
//                {
//                    errors.Add($"{file.FileName}: File type '{extension}' is not allowed.");
//                    continue;
//                }

//                // 5. File size check
//                var isVideo = AllowedVideoTypes.Contains(extension);
//                var maxSize = isVideo ? MaxVideoSize : MaxImageSize;
//                if (file.Length > maxSize)
//                {
//                    var limitMb = maxSize / (1024 * 1024);
//                    errors.Add($"{file.FileName}: Exceeds the {limitMb}MB size limit for {(isVideo ? "videos" : "images")}.");
//                    continue;
//                }

//                // 6. MIME type check (don't trust extension alone)
//                var allowedMimeTypes = isVideo
//                    ? new[] { "video/mp4", "video/quicktime", "video/x-msvideo", "video/webm", "video/x-matroska" }
//                    : new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };

//                if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
//                {
//                    errors.Add($"{file.FileName}: MIME type '{file.ContentType}' is not allowed.");
//                    continue;
//                }

//                // 7. Magic bytes check (verify actual file content, not just extension/mime)
//                if (!await IsValidFileSignatureAsync(file, isVideo))
//                {
//                    errors.Add($"{file.FileName}: File content does not match its extension.");
//                    continue;
//                }

//                // 8. Sanitize filename to prevent path traversal attacks
//                var safeFileName = Path.GetFileName(file.FileName); // strips any directory components
//                var fileName = $"{Guid.NewGuid()}_{safeFileName}";
//                var filePath = Path.Combine(_mediaRootPath, fileName);

//                try
//                {
//                    using var stream = new FileStream(filePath, FileMode.Create);
//                    await file.CopyToAsync(stream);

//                    mediaEntities.Add(new FileUploadDTO
//                    {
//                        Media_Date = DateTime.UtcNow,
//                        Media_Title = Path.GetFileNameWithoutExtension(file.FileName),
//                        Media_File_Name = fileName,
//                        Media_File_Location = filePath,

//                    });
//                }
//                catch (Exception ex)
//                {
//                    // 9. Clean up partial file if write failed
//                    if (System.IO.File.Exists(filePath))
//                        System.IO.File.Delete(filePath);

//                    errors.Add($"{file.FileName}: Failed to save file. {ex.Message}");
//                }
//            }

//            // 10. Return appropriate response
//            if (!mediaEntities.Any())
//                 response.errorMessage = "No files were uploaded successully.";
                 


//            if (errors.Any())
//                response.errorMessage = $"Some files failed to upload";
//            //return StatusCode(207, new { message = "Some files failed to upload.", uploaded = mediaEntities, errors });
            
//            response.errors = errors;
//            response.UploadedFiles = mediaEntities;

//            return response;
//        }

//        // Magic bytes validation — checks actual binary content of the file
//        private static async Task<bool> IsValidFileSignatureAsync(IFormFile file, bool isVideo)
//        {
//            await using var stream = file.OpenReadStream();
//            var header = new byte[8];
//            await stream.ReadAsync(header, 0, header.Length);

//            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

//            return extension switch
//            {
//                // Images
//                ".jpg" or ".jpeg" => header[0] == 0xFF && header[1] == 0xD8,
//                ".png" => header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47,
//                ".gif" => header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46,
//                ".webp" => header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46,

//                // Videos
//                ".mp4" or ".mov" => header[4] == 0x66 && header[5] == 0x74 && header[6] == 0x79 && header[7] == 0x70,
//                ".avi" => header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46,
//                ".webm" => header[0] == 0x1A && header[1] == 0x45 && header[2] == 0xDF && header[3] == 0xA3,

//                _ => false
//            };
//        }
//    }
//}
