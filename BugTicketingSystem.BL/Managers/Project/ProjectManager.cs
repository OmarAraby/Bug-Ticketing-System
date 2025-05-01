// ProjectManager.cs
using BugTicketingSystem.BL.Dtos.BugDtos;
using BugTicketingSystem.BL.Dtos.ProjectDtos;
using BugTicketingSystem.BL.Utils.Error;
using BugTicketingSystem.DL.Models;
using BugTicketingSystem.DL.Repository.ProjectRepository;
using BugTicketingSystem.DL.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Managers.Project
{
    public class ProjectManager : IProjectManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProjectCreateDto> _validator;

        public ProjectManager(
            IUnitOfWork unitOfWork,
            IValidator<ProjectCreateDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<APIResult<ProjectDetailsDto>> Create(ProjectCreateDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new APIResult<ProjectDetailsDto>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => new APIError
                    {
                        Code = e.ErrorCode,
                        Message = e.ErrorMessage
                    }).ToArray()
                };
            }

            var project = new DL.Models.Project
            {
                ProjectId = Guid.NewGuid(),
                ProjectName = dto.ProjectName,
                Description = dto.Description
            };

            await _unitOfWork.ProjectRepository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return new APIResult<ProjectDetailsDto>
            {
                Success = true,
                Data = MapToDetailsDto(project)
            };
        }

        public async Task<APIResult<IEnumerable<ProjectDetailsDto>>> GetAll()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            return new APIResult<IEnumerable<ProjectDetailsDto>>
            {
                Success = true,
                Data = projects.Select(MapToDetailsDto)
            };
        }

        public async Task<APIResult<ProjectDetailsDto>> GetDetails(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return new APIResult<ProjectDetailsDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "PROJECT_001", Message = "Project not found" } }
                };
            }

            return new APIResult<ProjectDetailsDto>
            {
                Success = true,
                Data = MapToDetailsDto(project)
            };
        }

        private ProjectDetailsDto MapToDetailsDto(DL.Models.Project project)
        {
            return new ProjectDetailsDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                Description = project.Description,
                BugsCount = project.Bugs?.Count ?? 0,
                Bugs = project.Bugs?.Select(b => new BugDetailsDto
                {
                    BugId = b.BugId,
                    Title = b.BugName,
                    Description = b.Description,
                    Status = b.Status.ToString()
                }).ToList()
            };
        }
    }
}