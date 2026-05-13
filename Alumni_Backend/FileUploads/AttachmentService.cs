using Alumni_Portal.FileUploads.DTO;


namespace Alumni_Portal.FileUploads
{
    public class AttachmentService
    {
        private readonly string _documentRootPath;

        private static readonly string[] AllowedExtensions =
        {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx"
    };

        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public AttachmentService(IWebHostEnvironment env)
        {
            var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
            _documentRootPath = Path.Combine(webRoot, "documents");

            if (!Directory.Exists(_documentRootPath))
                Directory.CreateDirectory(_documentRootPath);
        }

        public async Task<DocumentUploadResponseDTO> UploadDocument(DocumentUploadRequestDTO request)
        {
            var response = new DocumentUploadResponseDTO();

            var file = request.File;

            if (file == null || file.Length == 0)
            {
                response.ErrorMessage = "No file provided.";
                return response;
            }

            // 🔹 Get extensionmentD
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                response.ErrorMessage = $"File type '{extension}' is not allowed.";
                return response;
            }

            // 🔹 Get size
            if (file.Length > MaxFileSize)
            {
                response.ErrorMessage = "File exceeds 10MB limit.";
                return response;
            }

            // 🔹 Get MIME type
            var contentType = file.ContentType;

            // 🔹 Generate safe file name
            var safeFileName = Path.GetFileName(file.FileName);
            var newFileName = $"{Guid.NewGuid()}_{safeFileName}";
            var filePath = Path.Combine(_documentRootPath, newFileName);

            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                response.File = new DocumentDTO
                {
                    Attachment_Title = request.Title,
                    Attachment_Description = request.Description,
                    Attachment_File_Name = newFileName,
                    Attachment_File_Location = $"/documents/{newFileName}",
                 
                    Attachment_Size =(int)Math.Round(file.Length / (1024.0 * 1024.0), 2),
                    Attachment_Type = extension,
                    Attachment_Date = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                response.ErrorMessage = $"Upload failed: {ex.Message}";
            }

            return response;
        }
    }

}
