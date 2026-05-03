using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.OpenPortalPages.MainPage.Services.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions;
namespace Alumni_Portal.OpenPortalPages.MainPage.Respositories
{
    public class FeedPageRepository
    {
        private readonly PostDbContext _context;
        private readonly SharedDbContext _sharedContext;

        public FeedPageRepository(PostDbContext context, SharedDbContext sharedContext)
        {
            _context = context;
            _sharedContext = sharedContext;
        }

        public async Task<(List<PostFeedItemDTO> Posts, bool HasMore)> GetPaginatedFeedAsync(
            List<int>? postTypeId,
            DateTime? cursorDate,
            int cursorPostId,
            int pageSize,
            CancellationToken ct = default)
        {

            int fetchCount = pageSize + 1;

            var query = _context.Posts
                .AsNoTracking()
                .Where(p => p.Published_Date < DateTime.UtcNow);


            if (postTypeId != null && postTypeId.Count > 0)
                query = query.Where(p => p.Post_Type_ID.HasValue && postTypeId.Contains(p.Post_Type_ID.Value));


            if (cursorDate.HasValue)
            {
                query = query.Where(p =>
                    p.Published_Date < cursorDate.Value ||
                    (p.Published_Date == cursorDate.Value && p.Post_ID < cursorPostId));
            }


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


            var mentionsByPost = mentions
                .GroupBy(x => x.Post_ID)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Dto).ToList());

            var mediaByPost = media
                .GroupBy(x => x.Post_ID)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Dto).ToList());


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
       
        
        //public async Task<List<PostFeedItemDTO>> GetMainPagePostsAsync()
        //{
        //    var posts = await _context.Posts
        //        .AsNoTracking()
        //        .Where(p => p.Visible_ == true)
        //        .OrderByDescending(p => p.Published_Date)
        //        .Select(p => new PostFeedItemDTO
        //        {
        //            Post_ID = p.Post_ID,
        //            Post_Type_Value = p.Post_Type_Value,
        //            Post_Title = p.Post_Title,
        //            Post_Content = p.Post_Content,
        //            Published_Date = p.Published_Date,
        //        })
        //        .ToListAsync();
        //    return posts;
        //}

        public async Task<List<BannerPostDTO>> GetBannerPostsAsync(bool takeLimit, List<int>? postTypeIds )
        {
            IQueryable<Posts> query = _context.Posts
           .AsNoTracking()
           .Where(p => postTypeIds == null || (p.Post_Type_ID.HasValue && postTypeIds.Contains(p.Post_Type_ID.Value)))
           .Where(p => p.Is_Banner_Post == true)
           .OrderByDescending(e => e.Published_Date);
            if (takeLimit)
            {
                query = query.Take(5);
            }

            var result = await query
                .GroupJoin(
                    _context.Post_Media,
                    post => post.Post_ID,
                    media => media.Post_ID,
                    (post, mediaGroup) => new { post, mediaGroup }
                )
                .Select(x => new BannerPostDTO
                {
                    Post_ID = x.post.Post_ID,
                    Post_Type_Value = x.post.Post_Type_Value,
                    Post_Title = x.post.Post_Title,
                    Post_Content = x.post.Post_Content,
                    Published_Date = x.post.Published_Date,

                   
                    Media_File_Location = x.mediaGroup
                                .Select(m => m.Media_File_Location)
                                .FirstOrDefault()
                })
                .ToListAsync();

            return result;
        }





    }
}