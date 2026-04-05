using System.Diagnostics.Contracts;

namespace Alumni_Portal.FileUploads.DTO
{
    public class DocumentDTO
    {
        public string File_Name { get; set; }
        public string File_Type { get; set; }

        public int File_Size { get; set; }

        public DateTime Uploaded_Date { get; set; }

        public string File_Path { get; set; }
         
    }


}
