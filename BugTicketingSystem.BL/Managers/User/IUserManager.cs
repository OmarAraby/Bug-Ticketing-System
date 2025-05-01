using BugTicketingSystem.BL.Dtos.UserDtos;
using BugTicketingSystem.BL.Utils.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Managers.User
{
 
        public interface IUserManager
        {
            Task<APIResult> Register(UserRegisterDto registerDto);
            Task<APIResult<AuthResponseDto>> Login(UserLoginDto loginDto);
            Task<APIResult<UserDetailsDto>> GetUserDetails(Guid userId);
            //Task<APIResult<bool>> ChangePassword(Guid userId, string currentPassword, string newPassword);
        }
  
}
