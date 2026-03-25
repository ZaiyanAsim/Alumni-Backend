using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Alumni_Portal.FileUploads.DTO;
using Alumni_Portal.FileUploads;
using Shared.Custom_Exceptions.ExceptionClasses;

namespace Entity_Directories.Services
{
    public class PostService
    {
        private IPostRepository _postRepo;
        private SharedRepository _sharedRepo;
        //private FileService _fileService;
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

        public async Task<int> CreatePost(PostCreationDTO newPost)
        {
            if (newPost == null)
            {
                throw new ValidationException("Post data cannot be null");
            }
            
            int postId = await _postRepo.CreateAsync(newPost);
            return postId;


        }

        public async Task AddMedia(List<IFormFile> media, int postId)
        {
            //UploadResponseDTO response = await _fileService.UploadMedia(media);

            //if (response.errorMessage != null || (response.errors != null && response.errors.Count > 0))
            //{
            //    string errors = response.errorMessage ?? string.Join(", ", response.errors);
            //    throw new ValidationException($"Media upload failed: {errors}");
            //}

            //if (response.UploadedFiles != null )
            //{
            //    List<Post_Media> postMediaList = response.UploadedFiles.Select(file => new Post_Media
            //    {
            //        Post_Id=postId,
            //        Media_Title = file.Media_Title,
            //        Media_Date = file.Media_Date,
            //        Media_File_Location = file.Media_File_Location,
            //        Media_File_Name = file.Media_File_Name,
                    
            //    }).ToList();

            //    await _postRepo.AddMediaAsync(postMediaList);
            //}


           


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
