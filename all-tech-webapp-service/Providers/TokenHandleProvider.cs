using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace all_tech_webapp_service.Providers
{
    public class TokenHandleProvider
    {
        private const string Issuer = "https://accounts.google.com";

        public static string GetSubFromToken(string token)
        {
            token = token.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var subClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub");
            return subClaim?.Value;
        }

        public static Task ValidateToken(MessageReceivedContext context, TokenValidationParameters tokenValidationParameters)
        {
            Console.WriteLine("OnMessageReceived");
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.Request.Headers["Authorization"].ToString();

                var handler = new JwtSecurityTokenHandler();


                if (token.StartsWith("Bearer "))
                {
                    context.Token = token.Substring(7);
                    handler.ValidateToken(context.Token, tokenValidationParameters, out var validatedToken);
                    Console.WriteLine($"Received token: {context.Token}");
                }
            }
            return Task.CompletedTask;
        }

        public static TokenValidationParameters GetTokenValidationParamers()
        {
            //var googlePublicKeys = GetGooglePublicKeysAsync().Result;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://accounts.google.com",
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKeys = new List<SecurityKey>() { new JsonWebKey("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") } 
            };
            return tokenValidationParameters;
        }

        /*public static object ConfigureOptions(JwtBearerOptions options)
        {
            options.Authority = Issuer;
            options.TokenValidationParameters = GetTokenValidationParamers();
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context => TokenHandleProvider.ValidateToken(context, options.TokenValidationParameters),
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("OnAuthenticationFailed");
                    return Task.CompletedTask;
                },
            };
        }*/
    }
}
