using BugTicketingSystem.BL.Dtos.UserDtos;
using BugTicketingSystem.BL.Services;
using BugTicketingSystem.BL.Utils.Error;
using BugTicketingSystem.DL.Models;
using BugTicketingSystem.DL.Repository.UserRepository;
using BugTicketingSystem.DL.UnitOfWork;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace BugTicketingSystem.BL.Managers.User
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTService _jwtService;
        private readonly IValidator<UserRegisterDto> _registerValidator;
        private readonly IValidator<UserLoginDto> _loginValidator;

        public UserManager(
            IUnitOfWork unitOfWork,
            JWTService jwtService,
            IValidator<UserRegisterDto> registerValidator,
            IValidator<UserLoginDto> loginValidator)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        public async Task<APIResult> Register(UserRegisterDto dto)
        {
            // Validate input
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => new APIError
                    {
                        Code = e.ErrorCode,
                        Message = e.ErrorMessage
                    }).ToArray()
                };
            }

            // Debug: Force log the duplicate check
            var exists = await _unitOfWork.UserRepository.UserExists(dto.UserName, dto.Email);
            Console.WriteLine($"Duplicate check: {exists} | User: {dto.UserName}, Email: {dto.Email}");

            if (exists)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "USER_001", Message = "User already exists" } }
                };
            }

            // Proceed only if user doesn't exist
            var user = new DL.Models.User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            };

            try
            {
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "USER_001", Message = "User already exists" } }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during registration: {ex.Message}");
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_001", Message = "An unexpected error occurred during registration" } }
                };
            }

            return new APIResult<UserDetailsDto>
            {
                Success = true,
                Data = MapToDetailsDto(user)
            };
        }

        public async Task<APIResult<AuthResponseDto>> Login(UserLoginDto dto)
        {
            try
            {
                await _loginValidator.ValidateAndThrowAsync(dto);

                var user = await _unitOfWork.UserRepository.GetByEmailAsync(dto.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    return new APIResult<AuthResponseDto>
                    {
                        Success = false,
                        Errors = new[] { new APIError { Code = "AUTH_001", Message = "Invalid credentials" } }
                    };
                }

                var authResponse = _jwtService.GenerateToken(user);

                return new APIResult<AuthResponseDto>
                {
                    Success = true,
                    Data = authResponse
                };
            }
            catch (ValidationException ex)
            {
                return new APIResult<AuthResponseDto>
                {
                    Success = false,
                    Errors = ex.Errors.Select(e => new APIError
                    {
                        Code = "VALID_002",
                        Message = e.ErrorMessage
                    }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new APIResult<AuthResponseDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_002", Message = ex.Message } }
                };
            }
        }

        public async Task<APIResult<UserDetailsDto>> GetUserDetails(Guid userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new APIResult<UserDetailsDto>
                    {
                        Success = false,
                        Errors = new[] { new APIError { Code = "USER_002", Message = "User not found" } }
                    };
                }

                return new APIResult<UserDetailsDto>
                {
                    Success = true,
                    Data = MapToDetailsDto(user)
                };
            }
            catch (Exception ex)
            {
                return new APIResult<UserDetailsDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_003", Message = ex.Message } }
                };
            }
        }

        private UserDetailsDto MapToDetailsDto(BugTicketingSystem.DL.Models.User user)
        {
            return new UserDetailsDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.UserRoles.Select(r => r.Role.ToString()).ToList(),
                AssignedBugsCount = user.AssignedBugs?.Count ?? 0
            };
        }
    }
}
