using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Shared.Custom_Exceptions.ExceptionClasses;

namespace Entity_Directories.Services
{
    public class PostService
    {
        private IPostRepository _postRepo;
        private SharedRepository _sharedRepo;
        public PostService(IPostRepository postRepo, SharedRepository sharedRepo)
        {
            _postRepo = postRepo;
            _sharedRepo = sharedRepo;
        }


        public async Task<PaginatedResult<postDirectoryDTO>> GetPostsPaginated(PostFilters filters, int _page, int _limit)
        {


            if (_page > 0 || _limit > 0)
            {


                var query = _postRepo.GetPosts(filters);

                int count = await _sharedRepo.CountAsync(query);

                var posts = await query.
                     Skip((_page - 1) * _limit)
                     .Take(_limit)
                     .ToListAsync();

                return new PaginatedResult<postDirectoryDTO>
                {
                    data = posts,
                    totalRecords = count,
                    _page = _page,
                    _size = _limit

                };
            }



            throw new ValidationException("Page and Limit must be greater than 0");


        }

        public async Task<postDirectoryDTO?> GetPostById(int id)
        {
            return await _postRepo.GetByIdAsync(id);
        }

        public async Task<int> CreatePost(PostCreationDTO newPost)
        {
            if (newPost == null)
            {
                throw new ValidationException("Post data cannot be null");
            }
            
            int postId = await _postRepo.CreateAsync(newPost);
            return postId;


        }

        public async Task<List<Post_Media>> GetMediaByPostId(int postId)
        {
            return await _postRepo.GetMediaByPostIdAsync(postId);
        }

        public async Task AddMedia(List<Post_Media> mediaList)
        {
            await _postRepo.AddMediaAsync(mediaList);
        }


        public async Task<List<int>> DeletePostsBulk(List<int> postIDs)
        {


            return await _postRepo.DeleteBulkAsync(postIDs);

        }

        public async Task<MentionsResultDTO> PostMentions( string searchTerm, string type)
        {
            var individualQuery = _sharedRepo.MentionsIndividual(searchTerm).Take(10);
            var projectQuery = _sharedRepo.MentionsProject(searchTerm).Take(10);
            switch (type)
            {
                case "individual":
                    var individualMentions = await individualQuery.ToListAsync();
                    return new MentionsResultDTO
                    {
                        IndividualMentions = individualMentions,
                       
                    };
                case "project":
                    var projectMentions = await projectQuery.ToListAsync();
                    return new MentionsResultDTO
                    {
                        
                        ProjectMentions = projectMentions
                    };
                default:
                    var individualMentionsBoth = await individualQuery.ToListAsync();
                    var projectMentionsBoth = await projectQuery.ToListAsync();
                    return new MentionsResultDTO
                    {
                        IndividualMentions = individualMentionsBoth,
                        ProjectMentions = projectMentionsBoth
                    };
                            }
        }

    }
}
