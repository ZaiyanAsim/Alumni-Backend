using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.OpenPortalPages.MainPage.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Portal.OpenPortalPages.MainPage.Respositories
{
    public class FeedPageRepository
    {
        private readonly PostDbContext _context;

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
                .Where(p => p.Published_Date < DateTime.UtcNow);

            // ── Type filter ───────────────────────────────────────────────────────
            if (postTypeId.HasValue)
                query = query.Where(p => p.Post_Type_ID == postTypeId.Value);

            // ── Keyset cursor ─────────────────────────────────────────────────────
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

            if (pagePosts.Count == 0)
                return ([], false);

            var postIds = pagePosts.Select(p => p.Post_ID).ToList();

            // ── Mentions for this page only ───────────────────────────────────────
            var mentions = await _context.Post_Mentions
                .AsNoTracking()
                .Where(m => postIds.Contains(m.Post_ID))
                .Select(m => new
                {
                    m.Post_ID,
                    Dto = new PostMentionsDTO
                    {
                        Mention_ID = m.Mention_ID,
                        Mention_Type = m.Mention_Type,
                        Mention_Name = m.Mention_Name,
                    }
                })
                .ToListAsync(ct);

            // ── Media for this page only ──────────────────────────────────────────
            var media = await _context.Post_Media
                .AsNoTracking()
                .Where(m => postIds.Contains(m.Post_ID))
                .Select(m => new
                {
                    m.Post_ID,
                    Dto = new PostMediaDTO
                    {
                        Post_Media_ID = m.Post_Media_ID,
                        Media_Title = m.Media_Title,
                        Media_Description = m.Media_Description,
                        Media_File_Location = m.Media_File_Location,
                        Media_File_Name = m.Media_File_Name,
                    }
                })
                .ToListAsync(ct);

            // ── Group in memory by Post_ID (O(1) lookups) ─────────────────────────
            var mentionsByPost = mentions
                .GroupBy(x => x.Post_ID)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Dto).ToList());

            var mediaByPost = media
                .GroupBy(x => x.Post_ID)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Dto).ToList());

            // ── Stitch together ───────────────────────────────────────────────────
            var result = pagePosts.Select(p => new PostFeedItemDTO
            {
                Post_ID = p.Post_ID,
                Post_Type_Value = p.Post_Type_Value,
                Post_Title = p.Post_Title,
                Post_Tags = p.Post_Tags,
                Post_Content = p.Post_Content,
                Published_Date = p.Published_Date,
                Mentions = mentionsByPost.TryGetValue(p.Post_ID, out var mList) ? mList : new(),
                Media = mediaByPost.TryGetValue(p.Post_ID, out var mdList) ? mdList : new(),
            }).ToList();

            return (result, hasMore);
        }
    }
}