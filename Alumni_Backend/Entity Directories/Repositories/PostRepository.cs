
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Entity_Directories.Repositories.MappingExpressions;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Services.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Custom_Exceptions.ExceptionClasses;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Entity_Directories.Repositories
{
    public class PostRepository : IPostRepository

    {
        private readonly PostDbContext _context;

        public PostRepository(PostDbContext context)
        {
            _context = context;
        }
        public IQueryable<postDirectoryDTO> GetPosts(PostFilters filters)
        {
            var posts = _context.Posts
                          .Where(p => filters.Types == null || filters.Types.Count == 0 || filters.Types.Contains(p.Post_Type_ID) || filters.Association_Types.Contains(p.Post_Association_ID))
                          .Select(PostMappings.PostToDTO())
                          .OrderByDescending(p => p.Created_Date);

            return posts;
        }

        public async Task<postDirectoryDTO?> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Where(p => p.Post_ID == id)
                .Select(PostMappings.PostToDTO())
                .FirstOrDefaultAsync();
        }
        public async Task<List<int>> DeleteBulkAsync(List<int> postIds)
        {


            try
            {
                // Build parameterized query
                var parameters = postIds.Select((id, index) =>
                    new SqlParameter($"@p{index}", id)
                ).ToArray();

                var parameterNames = string.Join(",",
                    parameters.Select(p => p.ParameterName)
                );

                // Delete and get successfully deleted IDs
                var deletedIds = await _context.Database
                    .SqlQueryRaw<int>(
                        $@"DELETE FROM Posts
                   OUTPUT DELETED.Post_ID
                   WHERE Post_ID IN ({parameterNames})",
                        parameters
                    )
                    .ToListAsync();


                var failedIds = postIds.Except(deletedIds).ToList();

                return failedIds;
            }
            catch (Exception)
            {
                // If the entire delete fails, all IDs failed
                return postIds;
            }
        }

        public async Task<int> CreateAsync(PostCreationDTO postDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var post = PostMappings.CreateDtoToPost().Compile().Invoke(postDetails.Details);

                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();

                await AddMentionsAsync(postDetails.Mentions, post.Post_ID);

                await transaction.CommitAsync();
                return post.Post_ID;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task AddMentionsAsync(List<PostMentionsDTO> mentions, int post_ID)
        {
            if (mentions == null || !mentions.Any()) return;

            var postMentions = mentions.Select(mention => new Post_Mentions
            {
                Post_Id = post_ID,
                Mention_Id = mention.Mention_Id,
                Mention_Type = mention.Mention_Type,
                Mention_Name = mention.Mention_Name
            }).ToList();

            await _context.Post_Mentions.AddRangeAsync(postMentions);
            await _context.SaveChangesAsync(); // no try/catch here — let it bubble to CreateAsync
        }

        public async Task AddMediaAsync(List<Post_Media> media)
        {

            
            _context.Post_Media.AddRange(media);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Post_Media>> GetMediaByPostIdAsync(int postId)
        {
            return await _context.Post_Media
                .Where(m => m.Post_Id == postId)
                .ToListAsync();
        }


        

    }

}

   