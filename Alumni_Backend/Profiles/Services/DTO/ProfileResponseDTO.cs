
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

    public List<MethodologyDTO>? Methodologies { get; set; } = new List<MethodologyDTO>();

    }

    public class TechStackDTO
    {
        public int StackId { get; set; }
        public string? Layer { get; set; }
        public string Technology { get; set; } = string.Empty;
    }

    public class MethodologyDTO
    {
        public int MethodologyId { get; set; }
        public string Value { get; set; } = string.Empty;
    }

    public class IndividualSearchDTO
    {
        public int Individual_ID { get; set; }
        public string Individual_Name { get; set; } = string.Empty;
        public string? Individual_Email { get; set; }
        public string? Logo_Url { get; set; }
        public bool Individual_Is_Alumni { get; set; }
    }

    public class IndividualProjectDTO
    {
        public int Map_ID { get; set; }
        public int Project_ID { get; set; }
        public string Project_Academic_ID { get; set; } = string.Empty;
        public string Project_Name { get; set; } = string.Empty;
        public string? Project_Type { get; set; }
        public int? Project_Year { get; set; }
        public bool? Is_Mentored { get; set; }
        public bool? Is_Sponsored { get; set; }
        public string? Individual_Role { get; set; }
    }

    public record AddMemberRequest(int IndividualId, string Role);
    public record AddSponsorRequest(int IndividualId);
    public record AddTechStackRequest(string Technology_Value, string? Layer_Value);
    public record AddMethodologyRequest(string MethodologyValue);
    public record UpdateDescriptionRequest(string Project_Description);
    public record AddAttachmentLinkRequest(string Title, string Url, string? Description);

    public class ProjectRequestDTO
    {
        public int Request_ID { get; set; }
        public string? Request_Type_Value { get; set; }
        public int? Individual_ID { get; set; }
        public string? Individual_Name { get; set; }
        public string? Individual_Email { get; set; }
        public string? Individual_Contact_Number { get; set; }
        public string? Individual_LinkedIn_Url { get; set; }
        public string? Motivation_Statement { get; set; }
        public string? Status_Value { get; set; }
        public bool Is_Individual_Registered { get; set; }
        public DateTime Created_At { get; set; }
    }


    public class MemberDTO
    {
        public int MapId { get; set; }
        public int Individual_ID { get; set; }
        public string Name { get; set; }
        public string? role { get; set; }
        public string? email { get; set; }
        public string? Logo_Url { get; set; }

        public MemberDTO(int mapId, int individual_ID, string name, string? role, string? email, string? logo_Url)
        {
            MapId = mapId;
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

        public bool? Is_Mentorship_Available { get; init; }

        public bool? Is_Sponsorship_Available { get; init; }

        public string Logo_Url { get; set; }

        public string Video_Url { get; set; }

        public List<string> Tech_Stack { get; set; } = new();

        public List<string> Members { get; set; } = new();

    }


    public class ProjectResultsDTO
    {
        public int Result_ID { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }

        public int? Type_ID { get; set; }
        public string? Type_Value { get; set; }

        public string? Image_Url { get; set; }

        public string ? MetricValue { get; set; }

        public string ? MetricLabel { get; set; }

        public string? Tags { get; set; }

        public string? Link { get; set; }

        public DateTime Date { get; set; }

        public IFormFile? Image { get; set; }
    }

    public class ProjectDeliverablesDTO
    {
        public int Deliverable_ID { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }

        public int? Status_ID { get; set; }

        public int? Category_ID { get; set; }
        public string? Status_Value { get; set; }

        public string? Category_Value { get; set; }

        public DateTime Date { get; set; }
    }



}
