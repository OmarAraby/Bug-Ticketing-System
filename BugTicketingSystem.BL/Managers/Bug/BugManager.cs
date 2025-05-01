using BugTicketingSystem.BL.Dtos.BugDtos;
using BugTicketingSystem.BL.Utils.Error;
using BugTicketingSystem.DL.Models;
using BugTicketingSystem.DL.UnitOfWork;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Managers.Bug
{
    public class BugManager : IBugManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<BugCreateDto> _bugCreateValidator;
        private readonly IValidator<AssignUserToBugDto> _assignUserValidator; // Add validator for AssignUserToBugDto

        public BugManager(
            IUnitOfWork unitOfWork,
            IValidator<BugCreateDto> bugCreateValidator,
            IValidator<AssignUserToBugDto> assignUserValidator)
        {
            _unitOfWork = unitOfWork;
            _bugCreateValidator = bugCreateValidator;
            _assignUserValidator = assignUserValidator;
        }

        public async Task<APIResult<BugDetailsDto>> Create(BugCreateDto dto)
        {
            var validationResult = await _bugCreateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new APIResult<BugDetailsDto>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => new APIError
                    {
                        Code = e.ErrorCode ?? "VALIDATION_ERROR",
                        Message = e.ErrorMessage
                    }).ToArray()
                };
            }

            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(dto.ProjectId);
            if (project == null)
            {
                return new APIResult<BugDetailsDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "PROJECT_NOT_FOUND", Message = "Specified project does not exist" } }
                };
            }

            var bug = new DL.Models.Bug
            {
                BugId = Guid.NewGuid(),
                BugName = dto.Title,
                Description = dto.Description,
                Status = BugStatus.New,
                ProjectId = dto.ProjectId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.BugRepository.AddAsync(bug);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new APIResult<BugDetailsDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_ERROR", Message = "An error occurred while creating the bug" } }
                };
            }

            return new APIResult<BugDetailsDto>
            {
                Success = true,
                Data = MapToDetailsDto(bug)
            };
        }

        public async Task<APIResult<IEnumerable<BugListDto>>> GetAll()
        {
            var bugs = await _unitOfWork.BugRepository.GetAllAsync();
            return new APIResult<IEnumerable<BugListDto>>
            {
                Success = true,
                Data = bugs.Select(MapToListDto)
            };
        }

        public async Task<APIResult<BugDetailsDto>> GetDetails(Guid id)
        {
            var bug = await _unitOfWork.BugRepository.GetByIdAsync(id);
            if (bug == null)
            {
                return new APIResult<BugDetailsDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "BUG_NOT_FOUND", Message = "Bug not found" } }
                };
            }

            return new APIResult<BugDetailsDto>
            {
                Success = true,
                Data = MapToDetailsDto(bug)
            };
        }

        public async Task<APIResult<IEnumerable<BugListDto>>> GetByProjectId(Guid projectId)
        {
            var bugs = await _unitOfWork.BugRepository.GetByProjectIdAsync(projectId);
            return new APIResult<IEnumerable<BugListDto>>
            {
                Success = true,
                Data = bugs.Select(MapToListDto)
            };
        }

        public async Task<APIResult> AssignUserToBug(Guid bugId, AssignUserToBugDto dto)
        {
            // Validate the DTO
            var validationResult = await _assignUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => new APIError
                    {
                        Code = e.ErrorCode ?? "VALIDATION_ERROR",
                        Message = e.ErrorMessage
                    }).ToArray()
                };
            }

            // Validate bugId separately since it's not part of the DTO
            if (bugId == Guid.Empty)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "INVALID_BUGID", Message = "BugId must be a valid GUID" } }
                };
            }

            var bug = await _unitOfWork.BugRepository.GetByIdAsync(bugId);
            if (bug == null)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "BUG_NOT_FOUND", Message = "Bug not found" } }
                };
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(dto.UserId);
            if (user == null)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "USER_NOT_FOUND", Message = "User not found" } }
                };
            }

            var isAssigned = await _unitOfWork.BugUserRepository.IsUserAssignedToBug(bugId, dto.UserId);
            if (isAssigned)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "ALREADY_ASSIGNED", Message = "User is already assigned to this bug" } }
                };
            }

            try
            {
                await _unitOfWork.BugUserRepository.AssignUserToBug(bugId, dto.UserId, dto.Role);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_ERROR", Message = "An error occurred while assigning the user to the bug" } }
                };
            }

            return new APIResult
            {
                Success = true
            };
        }

        public async Task<APIResult> RemoveUserFromBug(Guid bugId, Guid userId)
        {
            // Validate bugId
            if (bugId == Guid.Empty)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "INVALID_BUGID", Message = "BugId must be a valid GUID" } }
                };
            }

            // Validate userId
            if (userId == Guid.Empty)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "INVALID_USERID", Message = "UserId must be a valid GUID" } }
                };
            }

            var bug = await _unitOfWork.BugRepository.GetByIdAsync(bugId);
            if (bug == null)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "BUG_NOT_FOUND", Message = "Bug not found" } }
                };
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "USER_NOT_FOUND", Message = "User not found" } }
                };
            }

            var isAssigned = await _unitOfWork.BugUserRepository.IsUserAssignedToBug(bugId, userId);
            if (!isAssigned)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "NOT_ASSIGNED", Message = "User is not assigned to this bug" } }
                };
            }

            try
            {
                await _unitOfWork.BugUserRepository.RemoveUserFromBug(bugId, userId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_ERROR", Message = "An error occurred while removing the user from the bug" } }
                };
            }

            return new APIResult
            {
                Success = true
            };
        }

        private BugDetailsDto MapToDetailsDto(DL.Models.Bug bug)
        {
            return new BugDetailsDto
            {
                BugId = bug.BugId,
                Title = bug.BugName,
                Description = bug.Description,
                Status = bug.Status.ToString(),
                CreatedAt = bug.CreatedAt,
                ResolvedAt = bug.ResolvedAt,
                Project = new ProjectBasicDto
                {
                    ProjectId = bug.ProjectId,
                    ProjectName = bug.Project?.ProjectName ?? string.Empty
                },
                Assignees = bug.Assignees?.Select(a => new UserBasicDto
                {
                    UserId = a.UserId,
                    UserName = a.User?.UserName ?? string.Empty,
                    Role = a.Role.ToString() // Map the Role from BugUser
                }).ToList() ?? new List<UserBasicDto>()
            };
        }

        private BugListDto MapToListDto(DL.Models.Bug bug)
        {
            return new BugListDto
            {
                BugId = bug.BugId,
                Title = bug.BugName,
                Status = bug.Status.ToString(),
                ProjectName = bug.Project?.ProjectName ?? string.Empty
            };
        }
    }
}