using Alumni_Portal.FileUploads.DTO;
using System.Globalization;
using System.Numerics;


namespace Alumni_Portal.Profiles.DTO
{
    public class ProjectProfileResponseDTO
    {
        
        public  MetaDataDTO Header_Data  { get; set; }
        public List<MemberDTO>? Members { get; set; } =new List<MemberDTO>();

        public List<ProjectDocumentDto>? Documents { get; set; }=new List<ProjectDocumentDto>();

        public List<ProjectResultsDTO>? Results { get; set; }=new List<ProjectResultsDTO>();

        public List<ProjectDeliverablesDTO>? Deliverables { get; set; }=new List<ProjectDeliverablesDTO>();

        public List<TechStackDTO>? TechStack { get; set; } = new List<TechStackDTO>();

    }

    public class TechStackDTO
    {
        public string? Layer { get; set; }
        public string Technology { get; set; } = string.Empty;
    }


    public class MemberDTO
    {

        public int Individual_ID { get; set; }
        public string Name { get; set; }

        public string? role { get; set; }

        public string? email { get; set; }

        public string? Logo_Url { get; set; }


        public MemberDTO(int individual_ID, string name, string? role, string? email, string? logo_Url)
        {
            Individual_ID = individual_ID;
            Name = name;
            this.role = role;
            this.email = email;
            Logo_Url = logo_Url;
        }
    }


    public class ProjectDocumentDto
    {
        public int AttachmentId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public string FileType { get; set; }

        public long FileSize { get; set; }

    }


    public class MetaDataDTO {

        public int Project_ID { get; init; }
        public required string Project_Academic_ID { get; init; }
        public required string Project_Name { get; init; }

        public string? Project_Description { get; init; }

        public int? Project_Year { get; init; }

        public required string Project_Type { get; init; }

        public string? Project_Category { get; init; }

        public bool? Is_Mentored { get; init; }

        public bool? Is_Sponsored { get; init; }
        public string Logo_Url { get; set; }

        public string Video_Url { get; set; }

    }



    public class ProjectResultsDTO
    {
        public int Seq_Number { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Type_Value { get; set; }

        public string? Image_Url { get; set; }

        public string ? MetricValue { get; set; }

        public string ? MetricLabel { get; set; }

        public string? Tags { get; set; }

        public string? Link { get; set; }
    }

    public class ProjectDeliverablesDTO
    {
        
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Status_Value { get; set; }

        public string? Category_Value { get; set; }

        public DateTime Date { get; set; }
    }






}
