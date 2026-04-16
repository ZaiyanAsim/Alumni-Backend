using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.OpenPortalPages.MainPage.Respositories;
using Alumni_Portal.OpenPortalPages.MainPage.Services.DTO;
using global::Entity_Directories.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Custom_Exceptions.ExceptionClasses;
using System.Text;




namespace Alumni_Portal.OpenPortalPages.MainPage.Services
{




    public class FeedService
    {
        private readonly FeedPageRepository _repo;

        public FeedService(FeedPageRepository repo) => _repo = repo;

        public async Task<PostFeedResultDTO> GetFeedAsync(PostFeedQueryDTO query, CancellationToken ct = default)
        {
            int pageSize = Math.Clamp(query.PageSize, 1, 100);

            var (posts, hasMore) = await _repo.GetFeedAsync(
                query.Post_Type_Id,
                query.CursorDate,
                query.CursorPostId ?? 0,
                pageSize,
                ct);

            PostFeedItemDTO? last = hasMore ? posts[^1] : null;

            return new PostFeedResultDTO
            {
                Posts = posts,
                NextCursorDate = last?.Published_Date,
                NextCursorPostId = last?.Post_ID,
            };
        }
      

    }
}



