using all_tech_webapp_service.Models.User;

namespace all_tech_webapp_service.Providers
{
    public interface ITokenHandlerProvider
    {
        string GetSubFromToken();

        UserCreateRequest GetUserCreateRequestFromToken();

        bool ValidateToken();

        void SetToken(string token);
    }
}