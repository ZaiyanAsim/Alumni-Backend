
//using Quartz;
//using Alumni_Portal.Infrastructure.Persistance;

//namespace Alumni_Portal.Entity_Directories.Jobs
//{
   

//    public class PublishPostJob : IJob
//    {
//        private readonly PostDbContext _context;
       

//        public PublishPostJob(PostDbContext context, RedisPostService redis)
//        {
//            _context = context;
//            _redis = redis;
//        }

//        public async Task Execute(IJobExecutionContext context)
//        {
//            var postId = context.JobDetail.JobDataMap.GetInt("postId");

//            var post = await _context.Posts.FindAsync(postId);

//            if (post == null)
//                return;


//            // publish to Redis
//            await _redis.AddPostAsync(post.Post_ID, post.Post_Type_Value, post.Published_Date);
          
          
//            await _context.SaveChangesAsync();
//        }
//    }

//}
