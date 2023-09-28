using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.DTO;

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
                .ThenInclude(pc => pc.Person)
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
            };
            
            _context.Projects.Add(project);            
            
            if(projectCreateDto.CandidateIds != null && projectCreateDto.CandidateIds.Any())
            {
                foreach(var candidateId in projectCreateDto.CandidateIds)
                {
                    var projectCandidate = new ProjectCandidate
                    {
                        Project = project,
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
                    // add other fields
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

            // refresh the state of the project
            existingProject.Name = projectUpdateDto.Name;
            existingProject.TechStack = projectUpdateDto.TechStack;
            existingProject.StartDate = projectUpdateDto.StartDate;
            existingProject.EndDate = projectUpdateDto.EndDate;
            existingProject.Comments = projectUpdateDto.Comments;

            // update connected candidates
            _context.ProjectCandidates.RemoveRange(existingProject.ProjectCandidates); // remove current connections
            
            if(projectUpdateDto.CandidateIds != null)
            {
                foreach (var candidateId in projectUpdateDto.CandidateIds)
                {
                    var projectCandidate = new ProjectCandidate
                    {
                        ProjectId = id,
                        PersonId = candidateId
                    };
                    _context.ProjectCandidates.Add(projectCandidate);
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