using CRUD_App.API.Middlewares.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CRUD_App.API.Middlewares
{
    public class TokenManager : ITokenManager
    {
        #region Properties
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;
        #endregion
        #region Constructor
        public TokenManager(IDistributedCache cache,
                IHttpContextAccessor httpContextAccessor,
                IOptions<JwtOptions> jwtOptions
            )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }
        #endregion
        #region Methods
        /// <summary>
        /// purpose : To Check any Current Token is active or not
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        /// <summary>
        /// purpose : To forcefully deactivate Current Token
        /// </summary>
        /// <returns></returns>
        public async Task DeactivateCurrentAsync()
        {
            string token = GetCurrentAsync();
            if (!string.IsNullOrEmpty(token))
            {
                await DeactivateAsync(token);
            }
        }
        /// <summary>
        /// purpose : by passing token string it will check isactive or not
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> IsActiveAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _cache.Remove(GetKey(token));
                return false;
            }
            var _token = GetKey(token);
            return await _cache.GetStringAsync(_token) == null;
        }
        /// <summary>
        /// purpose : by passing token string it will deactive that token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = null
                });

        /// <summary>
        /// purpose : To split token string with bearer and return only token string
        /// </summary>
        /// <returns></returns>
        public string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];
            char[] delimiterChars = { ' '};
           return authorizationHeader == StringValues.Empty
                ? string.Empty 
                : authorizationHeader.ToString().Split(delimiterChars)[1].ToString();
        }
        /// <summary>
        /// purpose : To get the Token key with deactivation
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";

        #endregion
    }

}
