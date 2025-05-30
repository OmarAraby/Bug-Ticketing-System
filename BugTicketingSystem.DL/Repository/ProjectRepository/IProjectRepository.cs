﻿using BugTicketingSystem.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.ProjectRepository
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
    }
}
