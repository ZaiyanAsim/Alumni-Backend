using Microsoft.Extensions.Caching.Memory;
using Alumni_Portal.Auth.Services.DTO;
using Alumni_Portal.RAID.Login;

namespace Alumni_Portal.Auth.Services.FeaturePermission
{
    public class AdminPermissionService
    {
        private readonly IMemoryCache _cache;
        private readonly LoginService _authService;
        private readonly ILogger<AdminPermissionService> _logger;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(8);

        public AdminPermissionService(
            IMemoryCache cache,
            LoginService authService,
            ILogger<AdminPermissionService> logger)
        {
            _cache = cache;
            _authService = authService;
            _logger = logger;
        }

      
        private static string CacheKey(string userId) => $"admin_perms:{userId}";

       
        public async Task<AdminAuthInfoDataDTO?> GetPermissionsAsync(
            string userId, string jwtToken)
        {
            var key = CacheKey(userId);

            if (_cache.TryGetValue(key, out AdminAuthInfoDataDTO? cached))
            {
                _logger.LogInformation("Permission cache HIT for userId {UserId}", userId);
                return cached;
            }

            _logger.LogInformation("Permission cache MISS for userId {UserId} — fetching", userId);

            var data = await _authService.GetAdminPermissionsAsync(jwtToken);

            if (data != null)
                _cache.Set(key, data, _cacheDuration);

            return data;
        }

       
        public void Invalidate(string userId) => _cache.Remove(CacheKey(userId));
    }
}