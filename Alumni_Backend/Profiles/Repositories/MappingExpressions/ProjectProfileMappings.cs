using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Profiles.DTO;
using System.Linq.Expressions;

namespace Alumni_Portal.Profiles.Repositories.MappingExpressions
{
    public static class ProjectProfileMapping
    {
        public static Expression<Func<Projects, MetaDataDTO>> ToMetaDataDTO =>
            p => new MetaDataDTO
            {
                Project_ID = p.Project_ID,
                Logo_Url = p.Logo_Url,
                Project_Academic_ID = p.Project_Academic_ID,
                Project_Name = p.Project_Name,
                Project_Type = p.Project_Type_Value,
                Video_Url = p.Video_Url,
                Project_Year = p.Project_Year,
                Project_Category = p.Project_Industries,
                Project_Description = p.Project_Description,
                Is_Mentored = p.Is_Mentored,
                Is_Sponsored = p.Is_Sponsored,
                Is_Mentorship_Available = p.Is_Mentorship_Available,
                Is_Sponsorship_Available = p.Is_Sponsorship_Available,
            };

        public static Expression<Func<Project_Attachments, ProjectDocumentDto>> ToProjectDocumentDto =>
            a => new ProjectDocumentDto
            {
                AttachmentId = a.Project_Attachment_ID,
                Title = a.Attachment_Title,
                Description = a.Attachment_Description ?? "",
                FileName = a.Attachment_File_Name ?? "",
                FileUrl = a.Attachment_File_Location ?? "",
                FileSize = a.Attachment_Size ?? 0,
                // ⚠️ Path.GetExtension cannot be used inside an Expression (not translatable by EF).
                // Set FileType after the query — see repo below.
                FileType = "",
            };

        public static Expression<Func<Project_Results, ProjectResultsDTO>> ToProjectResultsDTO =>
            r => new ProjectResultsDTO
            {
                Seq_Number = r.Result_Seq_Number ?? 0,
                Title = r.Result_Title,
                Description = r.Result_Description!,
                Type_Value = r.Result_Type_Value!,
                MetricLabel = r.Result_Metric_Label!,
                MetricValue = r.Result_Metric_Value!,
                Link = r.Result_Link!,
                Tags = r.Result_Tags!,
                Image_Url = r.Result_Image_Url!,
            };

        public static Expression<Func<Project_Tech_Stack, TechStackDTO>> ToTechStackDTO =>
            t => new TechStackDTO
            {
                Layer = t.Layer_Value,
                Technology = t.Technology_Value,
            };

        public static Expression<Func<Project_Delivarables, ProjectDeliverablesDTO>> ToProjectDeliverablesDTO =>
            d => new ProjectDeliverablesDTO
            {
                Title = d.Deliverable_Title,
                Description = d.Deliverable_Description ?? "",
                Status_Value = d.Deliverable_Status_Value!,
                Category_Value = d.Deliverable_Category_Value!,
                Date = d.Date,
            };


        public static class ProjectModelMapping
        {
            public static Project_Results ToResultModel(int projectId, ProjectResultsDTO dto) =>
                new Project_Results()
                {
                    Project_ID = projectId,
                    Result_Type_ID=dto.Type_ID ?? 0,
                    Result_Title = dto.Title,
                    Result_Description = dto.Description,
                    Result_Type_Value = dto.Type_Value!,
                    Result_Image_Url = dto.Image_Url,
                    Result_Seq_Number = dto.Seq_Number,
                    Result_Metric_Value = dto.MetricValue,
                    Result_Metric_Label = dto.MetricLabel,
                    Result_Link = dto.Link,
                    Result_Tags = dto.Tags,
                };

            public static Project_Delivarables ToDeliverableModel(int projectId, ProjectDeliverablesDTO dto) =>
                new Project_Delivarables()
                {
                    Project_ID = projectId,
                    Deliverable_Title = dto.Title,
                    Deliverable_Description = dto.Description,
                    Deliverable_Status_ID=dto.Status_ID ?? 0,
                    Deliverable_Category_ID=dto.Category_ID ?? 0,
                    Deliverable_Status_Value = dto.Status_Value!,
                    Deliverable_Category_Value = dto.Category_Value!,
                    Date = dto.Date,
                };
        }
    }
}
