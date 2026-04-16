namespace Alumni_Portal.FileUploads.DTO
{
    
    
    
    public class UploadResponseDTO
    {
        public List<FileUploadDTO> UploadedFiles { get; set; } = new List<FileUploadDTO>();
        public string errorMessage { get; set; } = string.Empty;

        public List<string> errors { get; set; } = new List<string>();
    }

    public class MediaUploadRequestDTO
    {
        public List<IFormFile> Files { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }



    public class FileUploadDTO
    {
        public DateTime Media_Date { get; set; }          // Media_Date
        public string Media_Title { get; set; }           // Media_Title
        public string? Media_Description { get; set; }    // Media_Description
        public string? Media_File_Location { get; set; }   // Media_File_Location
        public string? Media_File_Name { get; set; } 

    }

    
}
