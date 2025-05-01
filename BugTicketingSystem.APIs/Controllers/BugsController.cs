using BugTicketingSystem.BL.Dtos.BugDtos;
using BugTicketingSystem.BL.Managers.Bug;
using BugTicketingSystem.BL.Utils.Error;
using BugTicketingSystem.DL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BugsController : ControllerBase
    {
        private readonly IBugManager _bugManager;

        public BugsController(IBugManager bugManager)
        {
            _bugManager = bugManager;
        }

        [HttpPost]
        public async Task<Results<Ok<APIResult<BugDetailsDto>>, BadRequest<APIResult<BugDetailsDto>>>> Create([FromBody] BugCreateDto dto)
        {
            var result = await _bugManager.Create(dto);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }

        [HttpGet]
        public async Task<Results<Ok<APIResult<IEnumerable<BugListDto>>>, NotFound>> GetAll()
        {
            var result = await _bugManager.GetAll();
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.NotFound();
        }

        [HttpGet("{id}")]
        public async Task<Results<Ok<APIResult<BugDetailsDto>>, NotFound<APIResult<BugDetailsDto>>>> GetDetails(Guid id)
        {
            var result = await _bugManager.GetDetails(id);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }

        [HttpGet("project/{projectId}")]
        public async Task<Results<Ok<APIResult<IEnumerable<BugListDto>>>, NotFound<APIResult<IEnumerable<BugListDto>>>>>
            GetByProjectId(Guid projectId)
        {
            var result = await _bugManager.GetByProjectId(projectId);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }

        [HttpPost("{bugId}/assign")]
        public async Task<Results<Ok<APIResult>, BadRequest<APIResult>>> AssignUserToBug(Guid bugId, [FromBody] AssignUserToBugDto dto)
        {
            var result = await _bugManager.AssignUserToBug(bugId, dto);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }

        [HttpDelete("{bugId}/unassign/{userId}")]
        public async Task<Results<Ok<APIResult>, NotFound<APIResult>>> RemoveUserFromBug(Guid bugId, Guid userId)
        {
            var result = await _bugManager.RemoveUserFromBug(bugId, userId);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);


        }
    }
}