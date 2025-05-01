using BugTicketingSystem.BL.Dtos.ProjectDtos;
using BugTicketingSystem.BL.Managers.Project;
using BugTicketingSystem.BL.Utils.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectManager _projectManager;

        public ProjectsController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        [HttpPost]
        public async Task<Results<Ok<APIResult<ProjectDetailsDto>>, BadRequest<APIResult<ProjectDetailsDto>>>>
            Create([FromBody] ProjectCreateDto dto)
        {
            var result = await _projectManager.Create(dto);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }

        [HttpGet]
        public async Task<Results<Ok<APIResult<IEnumerable<ProjectDetailsDto>>>, NotFound>>
            GetAll()
        {
            var result = await _projectManager.GetAll();
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.NotFound();
        }

        [HttpGet("{id}")]
        public async Task<Results<Ok<APIResult<ProjectDetailsDto>>, NotFound<APIResult<ProjectDetailsDto>>>>
            GetDetails(Guid id)
        {
            var result = await _projectManager.GetDetails(id);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }
    }
}
