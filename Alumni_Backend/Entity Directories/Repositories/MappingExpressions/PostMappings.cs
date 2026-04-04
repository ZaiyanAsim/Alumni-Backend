using System.Linq.Expressions;
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services.DTO;
using Alumni_Portal.FileUploads.DTO;


namespace Entity_Directories.Repositories.MappingExpressions
{
    public class PostMappings
    {
        public static Expression<Func<Posts, postDirectoryDTO>> PostToDTO()
        {
            return i => new postDirectoryDTO
            {
                Post_ID = i.Post_ID,
                Post_Title = i.Post_Title,
                Post_Content = i.Post_Content,
                Post_Status = i.Status_Value,
                Post_Association = i.Post_Association_Value,
                Post_Type = i.Post_Type_Value,
                Post_Tags = i.Post_Tags,
                Created_By_Name = i.Created_By_Name,
                Created_Date = i.Created_Date,
                Published_Date = i.Published_Date
            };
        }

        public static Expression<Func<MetaData, Posts>> CreateDtoToPost()
        {
            return i => new Posts
            {
                Client_ID = 1,
                Campus_ID = 1,
                Post_Title = i.Post_Title,
                Post_Content = i.Post_Content,
                Status_ID = i.Status_ID,
                Status_Value = i.Status_Value,
                Post_Type_ID = i.Post_Type_ID,
                Post_Type_Value = i.Post_Type_Value ?? "General",
                Post_Tags = i.Post_Tags,
                Created_By_Name = i.Created_By_Name,
                Created_Date = i.Created_Date,
                Published_Date = i.Published_Date,
                Pusbished_By_Name = i.Created_By_Name ?? "Admin"
            };

        }

        }

        
}






