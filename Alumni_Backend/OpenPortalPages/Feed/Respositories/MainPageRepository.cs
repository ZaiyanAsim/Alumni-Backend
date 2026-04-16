using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.OpenPortalPages.MainPage.Services.DTO;
using Microsoft.EntityFrameworkCore;
namespace Alumni_Portal.OpenPortalPages.MainPage.Respositories
{
    public class FeedPageRepository
    {
        private PostDbContext _context;
        public FeedPageRepository(PostDbContext context) 
        {
            _context = context;
        }
        public async Task<(List<PostFeedItemDTO> Posts, bool HasMore)> GetFeedAsync(
        int? postTypeId,
        DateTime? cursorDate,
        int cursorPostId,
        int pageSize,
        CancellationToken ct = default)
        {
            // Fetch one extra row to know whether a next page exists
            int fetchCount = pageSize + 1;

            var query = _context.Posts
                .AsNoTracking()
                .Where(p => p.Published_Date<DateTime.UtcNow);

            // ── Type filter ───────────────────────────────────────────────────────
            if (postTypeId.HasValue)
                query = query.Where(p => p.Post_Type_ID == postTypeId.Value);

            
            if (cursorDate.HasValue)
            {
                query = query.Where(p =>
                    p.Published_Date < cursorDate.Value ||
                    (p.Published_Date == cursorDate.Value && p.Post_ID < cursorPostId));
            }

            // ── Sort + limit ──────────────────────────────────────────────────────
            var rawPosts = await query
                .OrderByDescending(p => p.Published_Date)
                .ThenByDescending(p => p.Post_ID)
                .Take(fetchCount)
                .Select(p => new
                {
                    p.Post_ID,
                    p.Post_Type_Value,
                    p.Post_Title,
                    p.Post_Tags,
                    p.Post_Content,
                    p.Published_Date,
                    
                })
                .ToListAsync(ct);

            bool hasMore = rawPosts.Count == fetchCount;
            var pagePosts = rawPosts.Take(pageSize).ToList();

            if (!pagePosts.Any())
                return ([], false);

            var postIds = pagePosts.Select(p => p.Post_ID).ToList();

           
            var mentions = await _context.Post_Mentions
                .AsNoTracking()
                .Where(m => postIds.Contains(m.Post_Id))
                .Select(m => new PostMentionsDTO
                {
                    Mention_Id = m.Mention_Id,
                    Mention_Type = m.Mention_Type,
                    Mention_Name = m.Mention_Name,
                })
                .ToListAsync(ct);

            var media = await _context.Post_Media
                .AsNoTracking()
                .Where(m => postIds.Contains(m.Post_Id))
                .Select(m => new PostMediaDTO
                {
                    Post_Media_Id = m.Post_Media_Id,
                    Media_Title = m.Media_Title,
                    Media_Description = m.Media_Description,
                   
                    Media_File_Location = m.Media_File_Location,
                    Media_File_Name = m.Media_File_Name,
                    
                })
                .ToListAsync(ct);

         
            var mentionsByPost = mentions.ToLookup(m => m.Mention_Id);  // adjust FK name
            var mediaByPost = media.ToLookup(m => m.Post_Media_Id);  // adjust FK name

            var result = pagePosts.Select(p => new PostFeedItemDTO
            {
                Post_ID = p.Post_ID,
                Post_Type_Value = p.Post_Type_Value,
                Post_Title = p.Post_Title,
                Post_Tags = p.Post_Tags,
                Post_Content = p.Post_Content,
                Published_Date = p.Published_Date,
                Mentions = mentionsByPost[p.Post_ID].ToList(),
                Media = mediaByPost[p.Post_ID].ToList(),
            }).ToList();
                

            return (result, hasMore);
        }
    }
}
