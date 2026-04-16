//using Alumni_Portal.Infrastructure.Persistance;
//using Alumni_Portal.Shared.services.DTO;
//using Microsoft.EntityFrameworkCore;

//namespace Alumni_Portal.Shared.services.Redis
//{
//    public class PostCacheWarmupService : IHostedService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;
//        private readonly RedisPostService _redis;
//        private readonly ILogger<PostCacheWarmupService> _logger;

//        public PostCacheWarmupService(
//            IServiceScopeFactory scopeFactory,
//            RedisPostService redis,
//            ILogger<PostCacheWarmupService> logger)
//        {
//            _scopeFactory = scopeFactory;
//            _redis = redis;
//            _logger = logger;
//        }

//        public async Task StartAsync(CancellationToken ct)
//        {
           

            
//                long count = await _redis.GetPostCountAsync("all");
//                if (count > 0) return;

//                _logger.LogInformation("Cache empty — warming from DB...");

//                using var scope = _scopeFactory.CreateScope();
//                var db = scope.ServiceProvider.GetRequiredService<PostDbContext>();

          
//            var posts = await  db.Posts
//                .Where(p => p.Published_Date <= DateTime.UtcNow)
//                .OrderByDescending(p => p.Published_Date)
//                .Select(p => new PostRedisInfoDTO{
//                    postId=p.Post_ID, 
//                    postType=p.Post_Type_Value, 
//                    publishedDate=p.Published_Date })
//                .Take(1000) 
//                .ToListAsync();

//                foreach (var post in posts)
//                    await _redis.AddPostAsync(post.postId, post.postType, post.publishedDate);

//              var distinctTypes = posts.Select(p => p.postType).Distinct();
//              foreach (var type in distinctTypes)
//               await _redis.RegisterTypeAsync(type)
             
            
            
//        }

//        public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
//    }
//}
