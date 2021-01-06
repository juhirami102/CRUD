using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRUD_App.Data.Entity;
using CRUD_App.Entity.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CRUD_App
{
    public static class TokenBuilder
    {
        #region Properties
        internal static TokenValidationParameters tokenValidationParams;
        public static SigningCredentials signingCredentials = null;
        //Construct our JWT authentication paramaters then inject the parameters into the current TokenBuilder instance
        // If injecting an RSA key for signing use this method
        // Be weary of common jwt trips: https://trustfoundry.net/jwt-hacking-101/ and https://www.sjoerdlangkemper.nl/2016/09/28/attacking-jwt-authentication/
        //public static void ConfigureJwtAuthentication(this IServiceCollection services, RSAParameters rsaParams)
        #endregion
        /// <summary>
        /// purpose : extension method of IServicecollection to configure JWT 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            string keyString = configuration["jwt:secretKey"].ToString(); //"401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed";
            byte[] symmetricKeyBytes = Encoding.ASCII.GetBytes(keyString);
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(symmetricKeyBytes);
            signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);


            tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["jwt:issuer"].ToString(),
                ValidateLifetime = true,
                ValidAudience = configuration["jwt:audience"].ToString(),
                ValidateAudience = true,
                RequireSignedTokens = true,
                // Use our signing credentials key here
                // optionally we can inject an RSA key as
                //IssuerSigningKey = new RsaSecurityKey(rsaParams),
                IssuerSigningKey = signingCredentials.Key,
                ClockSkew = TimeSpan.FromMinutes(0)
            };
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParams;
             #if PROD || UAT
                options.IncludeErrorDetails = false;
#elif DEBUG
                options.RequireHttpsMetadata = false;
#endif
            });
        }
        /// <summary>
        /// purpose : Creating JWT Token method from Login User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <param name="audienceUri"></param>
        /// <param name="issuerUri"></param>
        /// <param name="applicationId"></param>
        /// <param name="expires"></param>
        /// <param name="deviceId"></param>
        /// <param name="isReAuthToken"></param>
        /// <returns></returns>
        public static string CreateJsonWebToken(
               User userEntity,
               IEnumerable<string> roles,
               string audienceUri,
               string issuerUri,
               Guid applicationId,
               DateTime expires,
               string deviceId = null,
               bool isReAuthToken = false)
        {
            var claims = new List<Claim>();
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                claims.Add(new Claim("FirstName", userEntity.FirstName));
                claims.Add(new Claim("UserId", userEntity.UserId.ToString()));
                claims.Add(new Claim("LastName", userEntity.LastName));
                claims.Add(new Claim("EmailAddress", userEntity.EmailAddress));
                claims.Add(new Claim("MobileNumber", userEntity.MobileNumber));
                claims.Add(new Claim("RoleId", Convert.ToString(userEntity.RoleId)));
            }
            var head = new JwtHeader();
            var payload = new JwtPayload(claims.ToArray());
            var jwt = new JwtSecurityToken(issuerUri, audienceUri, claims, DateTime.UtcNow, expires, signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
    /// <summary>
    /// Class for defining policies with specified roles
    /// </summary>
    ///
    public static class Policies
    {
        #region Properties
        public const string Admin = "Admin";
        public const string TUOwner = "TUOwner";
        public const string Insurance = "Insurance";
        public const string User = "User";
        public const string All = "AllPolicies";
        public static string[] strRoles = { "Admin", "TUOwner", "Customer", "Insurance" };
        #endregion
        #region Methods
        /// <summary>
        /// Method to get Admin Policy
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }
        /// <summary>
        /// Method to get User Policy
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(User).Build();
        }
        /// <summary>
        /// Method to get TUOwner Policy
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy TUOwnerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(TUOwner).Build();
        }
        /// <summary>
        /// Method to get Insurance Policy
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy InsurancePolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Insurance).Build();
        }
        /// <summary>
        /// Method to get All Policies for all roles
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AllPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(strRoles).Build();
        }
        #endregion
    }
}
