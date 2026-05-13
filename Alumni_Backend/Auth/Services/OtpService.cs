// Services/RegistrationOtpService.cs
using Microsoft.Extensions.Caching.Memory;

namespace Alumni_Portal.Auth.Services
{
    public class RegistrationOtpService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan ResendCooldown = TimeSpan.FromSeconds(60);

        public RegistrationOtpService(IMemoryCache cache) => _cache = cache;

        public string Generate(int requestId)
        {
            // Rate-limit resends
            if (_cache.TryGetValue<DateTime>(CooldownKey(requestId), out var lastSent)
                && DateTime.UtcNow - lastSent < ResendCooldown)
            {
                var wait = ResendCooldown - (DateTime.UtcNow - lastSent);
                throw new InvalidOperationException(
                    $"Please wait {(int)wait.TotalSeconds} seconds before requesting another OTP.");
            }

            var otp = new Random().Next(100000, 999999).ToString();
            _cache.Set(Key(requestId), otp, Lifetime);
            _cache.Set(CooldownKey(requestId), DateTime.UtcNow, ResendCooldown);
            return otp;
        }

        public bool Verify(int requestId, string otp)
        {
            if (!_cache.TryGetValue<string>(Key(requestId), out var stored)) return false;
            if (stored != otp) return false;

            _cache.Remove(Key(requestId)); 
            return true;
        }

        private static string Key(int id) => $"reg-otp:{id}";
        private static string CooldownKey(int id) => $"reg-otp-cooldown:{id}";
    }
}