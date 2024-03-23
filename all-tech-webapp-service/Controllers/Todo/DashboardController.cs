using all_tech_webapp_service.Services.Todo.DashBoard;
using Microsoft.AspNetCore.Mvc;

namespace all_tech_webapp_service.Controllers.Todo
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        }

        [HttpGet]
        [Route("{userId:guid}")]
        public async Task<IActionResult> GetUserDashboardData([FromRoute] Guid userId)
        {
            var todoGroupResponse = await _dashboardService.GetUserDashBoardData(userId);
            return Ok(todoGroupResponse);
        }
    }
}
