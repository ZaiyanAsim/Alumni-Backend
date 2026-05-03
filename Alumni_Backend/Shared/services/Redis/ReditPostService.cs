//using StackExchange.Redis;

//namespace Alumni_Portal.Shared.services.Redis
//{
//    public class RedisPostService
//    {
//        private readonly IDatabase _db;

//        public RedisPostService(IConnectionMultiplexer redis)
//        {
//            _db = redis.GetDatabase();
//        }


//        public async Task RegisterTypeAsync(string postType)
//        {
//            await _db.SetAddAsync("posts:types", postType);
//        }

//        public async Task AddPostAsync(int postId, string postType, DateTime publishedDate)
//        {
//            double score = publishedDate.ToUniversalTime()
//                .Subtract(DateTime.UnixEpoch)
//                .TotalSeconds;

          
//            var batch = _db.CreateBatch();
//            var t1 = batch.SortedSetAddAsync("posts:all", postId, score);
//            var t2 = batch.SortedSetAddAsync($"posts:{postType}", postId, score);
//            batch.Execute();
//            await Task.WhenAll(t1, t2);
//        }


//        public async Task RemovePostsAsync(List<int> postIds)
//        {
            
//            var typeMembers = await _db.SetMembersAsync("posts:types");
//            var typeKeys = typeMembers.Select(t => $"posts:{t}").ToList();

            
//            typeKeys.Add("posts:all");

//            var redisValues = postIds.Select(id => (RedisValue)id).ToArray();

//            var batch = _db.CreateBatch();
//            var tasks = new List<Task>();

//            foreach (var key in typeKeys)
//            {
                
//                tasks.Add(batch.SortedSetRemoveAsync(key, redisValues));
//            }

//            batch.Execute();
//            await Task.WhenAll(tasks);
//        }

//        public async Task<(List<int> ids, long redisTotal)> GetPagedIdsAsync(
//            string postType, int page, int pageSize)
//        {
//            string key = postType == "all" ? "posts:all" : $"posts:{postType}";

//            long start = (long)(page - 1) * pageSize;
//            long stop = start + pageSize - 1;

//            var batch = _db.CreateBatch();
//            var rangeTask = batch.SortedSetRangeByRankAsync(key, start, stop, Order.Descending);
//            var countTask = batch.SortedSetLengthAsync(key);
//            batch.Execute();

//            var range = await rangeTask;
//            var count = await countTask;

//            return (range.Select(v => (int)(long)v).ToList(), count);
//        }
//    }
//}