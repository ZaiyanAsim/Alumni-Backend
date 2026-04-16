using Quartz.Impl.Triggers;
using System.Diagnostics.Contracts;

namespace Alumni_Portal.FileUploads.DTO
{

    public class DocumentUploadRequestDTO
    {
        public IFormFile File { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class DocumentUploadResponseDTO
    {
        public DocumentDTO File { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class DocumentDTO
    {
        public string Attachment_Title { get; set; }
        public string Attachment_Type { get; set; }
        public string Attachment_Description { get; set; }
        public int Attachment_Size { get; set; }

        public DateTime Attachment_Date { get; set; }

        public string Attachment_File_Location { get; set; }

        public string Attachment_File_Name { get; set; }

    }
    




}
