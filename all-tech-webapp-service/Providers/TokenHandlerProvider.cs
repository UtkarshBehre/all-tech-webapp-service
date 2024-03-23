using System.IdentityModel.Tokens.Jwt;
using all_tech_webapp_service.Models.User;
using all_tech_webapp_service.Properties;
using Microsoft.IdentityModel.Tokens;

namespace all_tech_webapp_service.Providers
{
    public class TokenHandlerProvider : ITokenHandlerProvider
    {
        private readonly IWebHostEnvironment _env;
        private readonly JwtSecurityTokenHandler _handler;
        private readonly string _aud;
        private readonly string _iss;

        private JwtSecurityToken _jwtToken;

        public TokenHandlerProvider(IWebHostEnvironment env, ConfigurationManager configuration)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _aud = configuration.GetValue<string>(Constants.AUDIENCE) ?? throw new ArgumentNullException(nameof(Constants.AUDIENCE));
            _iss = configuration.GetValue<string>(Constants.ISSUER) ?? throw new ArgumentNullException(nameof(Constants.ISSUER));
            _handler = new JwtSecurityTokenHandler();
        }

        public void SetToken(string token)
        {
/*            if (_env.IsDevelopment())
            {
                return;
            }*/
            
            _jwtToken = _handler.ReadJwtToken(token.Substring("Bearer ".Length));
        }

        public string GetSubFromToken()
        {
           /* if (_env.IsDevelopment())
            {
                return "";
            }*/
            return _jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
        }

        public UserCreateRequest GetUserCreateRequestFromToken()
        {
            /*   if (_env.IsDevelopment())
            {
                return "";
            }*/
            var userCreateRequest = new UserCreateRequest
            {
                Email = _jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value,
                FirstName = _jwtToken.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value,
                LastName = _jwtToken.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value,
                GoogleId = _jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value
            };

            return userCreateRequest;
        }

        public void ValidateToken()
        {
/*            if (_env.IsDevelopment())
            {
                return true;
            }*/

            var exp = _jwtToken.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
            var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp));
            if (expDate < DateTimeOffset.Now) {
                throw new SecurityTokenExpiredException(); 
            }

            var iss = _jwtToken.Claims.FirstOrDefault(x => x.Type == "iss")?.Value;
            if (!string.Equals(iss, _iss, StringComparison.OrdinalIgnoreCase))
            {
                var message = $"Invalid issuer: {iss}";
                throw new SecurityTokenInvalidIssuerException(message);
            }

            var aud = _jwtToken.Claims.FirstOrDefault(x => x.Type == "aud")?.Value;
            if (!string.Equals(aud, _aud, StringComparison.OrdinalIgnoreCase))
            {
                var message = $"Invalid audience: {aud}";
                throw new SecurityTokenInvalidAudienceException(message);
            }
        }
    }
}
