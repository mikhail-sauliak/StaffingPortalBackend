using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectCandidates)
                .ThenInclude(pc => pc.Person) // Включает информацию о кандидатах
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetProjects()
        {
            var projects = await _context.Projects
                .Include(p => p.ProjectCandidates)
                .ThenInclude(pc => pc.Person)
                .ToListAsync();

            var projectDtos = projects.Select(p => new ProjectReadDto
            {
                Id = p.Id,
                Name = p.Name,
                TechStack = p.TechStack,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Comments = p.Comments,
                Candidates = p.ProjectCandidates.Select(pc => new PersonReadDto
                {
                    Id = pc.PersonId,
                    FirstName = pc.Person.FirstName,
                    LastName = pc.Person.LastName,
                    Location = pc.Person.Location
                    // Заполните другие свойства
                }).ToList()
            }).ToList();

            return projectDtos;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectReadDto>> CreateProject(ProjectCreateDto projectCreateDto)
        {
            var project = new Project
            {
                Name = projectCreateDto.Name,
                TechStack = projectCreateDto.TechStack,
                StartDate = projectCreateDto.StartDate,
                EndDate = projectCreateDto.EndDate,
                Comments = projectCreateDto.Comments
                // Заполните другие свойства
            };
            
            _context.Projects.Add(project);
            
            // Если CandidateIds предоставлены, создайте связи между проектом и кандидатами
            if(projectCreateDto.CandidateIds != null && projectCreateDto.CandidateIds.Any())
            {
                foreach(var candidateId in projectCreateDto.CandidateIds)
                {
                    var projectCandidate = new ProjectCandidate
                    {
                        Project = project, // Установите связь с только что созданным проектом
                        PersonId = candidateId
                    };
                    _context.ProjectCandidates.Add(projectCandidate);
                }
            }
            
            await _context.SaveChangesAsync();

            var projectReadDto = new ProjectReadDto
            {
                Id = project.Id,
                Name = project.Name,
                TechStack = project.TechStack,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Comments = project.Comments,
                Candidates = project.ProjectCandidates?.Select(pc => new PersonReadDto
                {
                    Id = pc.PersonId,
                    FirstName = pc.Person.FirstName,
                    LastName = pc.Person.LastName,
                    Location = pc.Person.Location
                    // Заполните другие свойства
                }).ToList()?? new List<PersonReadDto>()
            };
            
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectUpdateDto projectUpdateDto)
        {
            var existingProject = await _context.Projects
                .Include(p => p.ProjectCandidates)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProject == null)
            {
                return NotFound();
            }

            // Обновление основной информации проекта
            existingProject.Name = projectUpdateDto.Name;
            existingProject.TechStack = projectUpdateDto.TechStack;
            existingProject.StartDate = projectUpdateDto.StartDate;
            existingProject.EndDate = projectUpdateDto.EndDate;
            existingProject.Comments = projectUpdateDto.Comments;

            // Обновление связанных кандидатов
            _context.ProjectCandidates.RemoveRange(existingProject.ProjectCandidates); // Удаление существующих связей
            
            if(projectUpdateDto.CandidateIds != null) // Убедитесь, что CandidateIds не null
            {
                foreach (var candidateId in projectUpdateDto.CandidateIds)
                {
                    var projectCandidate = new ProjectCandidate
                    {
                        ProjectId = id,
                        PersonId = candidateId
                    };
                    _context.ProjectCandidates.Add(projectCandidate); // Добавление новых связей
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}