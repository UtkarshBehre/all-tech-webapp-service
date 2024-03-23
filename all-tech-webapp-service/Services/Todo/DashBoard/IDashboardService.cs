using all_tech_webapp_service.Models.Todo;

namespace all_tech_webapp_service.Services.Todo.DashBoard
{
    public interface IDashboardService
    {
        /// <summary>
        /// Get user dashboard data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AllItemsResponse> GetUserDashBoardData(Guid userId);
    }
}
