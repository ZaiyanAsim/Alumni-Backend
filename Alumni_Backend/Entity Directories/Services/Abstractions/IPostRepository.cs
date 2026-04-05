
using Entity_Directories.Services.DTO;
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;

namespace Entity_Directories.Services.Abstractions
{
    public interface IPostRepository
    {
        public  IQueryable<postDirectoryDTO> GetPosts(PostFilters fiters);
        public Task<List<int>> DeleteBulkAsync(List<int> ids);

        public Task<int> CreateAsync(PostCreationDTO postDetails);

        public Task AddMediaAsync(List<Post_Media> media);
        public Task<List<Post_Media>> GetMediaByPostIdAsync(int postId);

        public Task<postDirectoryDTO?> GetByIdAsync(int id);


    }
}
