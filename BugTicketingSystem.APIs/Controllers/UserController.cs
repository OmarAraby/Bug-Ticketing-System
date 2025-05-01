using BugTicketingSystem.BL.Dtos.UserDtos;
using BugTicketingSystem.BL.Managers.User;
using BugTicketingSystem.BL.Utils.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<Results<Ok<APIResult>, BadRequest<APIResult>>> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _userManager.Register(dto);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<Results<Ok<AuthResponseDto>, UnauthorizedHttpResult>> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var result = await _userManager.Login(loginDto);

                if (result.Success && result.Data != null)
                {
                    return TypedResults.Ok(result.Data);
                }

                return TypedResults.Unauthorized();
            }
            catch (Exception)
            {
                return TypedResults.Unauthorized();
            }
        }

        //[HttpGet("{userId}")]
        //[Authorize]
        //public async Task<IActionResult> GetUserDetails(Guid userId)
        //{
        //    var result = await _userManager.GetUserDetails(userId);

        //    if (!result.Success)
        //    {
        //        return NotFound(new { result.Errors });
        //    }

        //    return Ok(result.Data);
        //}


    }

}
