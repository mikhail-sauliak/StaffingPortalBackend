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
        public async Task<ActionResult<ProjectReadDto>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectCandidates)
                .ThenInclude(pc => pc.Person)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var projectReadDto = new ProjectReadDto
            {
                Id = project.Id,
                Name = project.Name,
                TechStack = project.TechStack,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Comments = project.Comments,
                Search = project.Search,
                Signed = project.Signed,
                PositionSigned = project.PositionSigned,
                PositionClosed = project.PositionClosed,
                Location = project.Location,
                Stream = project.Stream,
                Level = project.Level,
                Priority = project.Priority,
                Attachment = project.Attachment,
                Candidates = project.ProjectCandidates.Select(pc => new PersonReadDto
                {
                    Id = pc.Person.Id,
                    FirstName = pc.Person.FirstName,
                    LastName = pc.Person.LastName,
                    // ... other fields
                }).ToList()
            };

            return projectReadDto;
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
                Search = p.Search,
                Signed = p.Signed,
                PositionSigned = p.PositionSigned,
                PositionClosed = p.PositionClosed,
                Location = p.Location,
                Stream = p.Stream,
                Level = p.Level,
                Priority = p.Priority,
                Attachment = p.Attachment,
                Candidates = p.ProjectCandidates.Select(pc => new PersonReadDto
                {
                    Id = pc.PersonId,
                    FirstName = pc.Person.FirstName,
                    LastName = pc.Person.LastName,
                    Location = pc.Person.Location
                    // add other fields
                }).ToList()
            }).ToList();

            return projectDtos;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectReadDto>> CreateProject([FromForm] ProjectCreateDto projectCreateDto)
        {
            if (projectCreateDto == null)
            {
                return BadRequest("Project data must not be null");
            }
            var project = new Project
            {
                Name = projectCreateDto.Name,
                TechStack = projectCreateDto.TechStack,
                StartDate = projectCreateDto.StartDate,
                EndDate = projectCreateDto.EndDate,
                Comments = projectCreateDto.Comments,
                Search = projectCreateDto.Search,
                Signed = projectCreateDto.Signed,
                PositionSigned = projectCreateDto.PositionSigned,
                PositionClosed = projectCreateDto.PositionClosed,
                Location = projectCreateDto.Location,
                Stream = projectCreateDto.Stream,
                Level = projectCreateDto.Level,
                Priority = projectCreateDto.Priority,
                Attachment = projectCreateDto.Attachment
            };

            _context.Projects.Add(project);

            if (projectCreateDto.CandidateIds != null && projectCreateDto.CandidateIds.Any())
            {
                foreach (var candidateId in projectCreateDto.CandidateIds)
                {
                    var projectCandidate = new ProjectCandidate
                    {
                        Project = project,
                        PersonId = candidateId
                    };
                    _context.ProjectCandidates.Add(projectCandidate);
                }
            }

             try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            var projectReadDto = new ProjectReadDto
            {
                Id = project.Id,
                Name = project.Name,
                TechStack = project.TechStack,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Comments = project.Comments,
                Search = project.Search,
                Signed = project.Signed,
                PositionSigned = project.PositionSigned,
                PositionClosed = project.PositionClosed,
                Location = project.Location,
                Stream = project.Stream,
                Level = project.Level,
                Priority = project.Priority,
                Attachment = project.Attachment,
                Candidates = project.ProjectCandidates?.Select(pc => new PersonReadDto
                {
                    Id = pc.PersonId,
                    FirstName = pc.Person.FirstName,
                    LastName = pc.Person.LastName,
                    Location = pc.Person.Location
                    // add other fields
                }).ToList() ?? new List<PersonReadDto>()
            };

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromForm] ProjectUpdateDto projectUpdateDto)
        {
            if (projectUpdateDto == null)
            {
                return BadRequest("Project data must not be null");
            }

            var existingProject = await _context.Projects
                .Include(p => p.ProjectCandidates)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProject == null)
            {
                return NotFound("Project not found");
            }

            existingProject.Name = projectUpdateDto.Name;
            existingProject.TechStack = projectUpdateDto.TechStack;
            existingProject.StartDate = projectUpdateDto.StartDate;
            existingProject.EndDate = projectUpdateDto.EndDate;
            existingProject.Comments = projectUpdateDto.Comments;
            existingProject.Search = projectUpdateDto.Search;
            existingProject.Signed = projectUpdateDto.Signed;
            existingProject.PositionSigned = projectUpdateDto.PositionSigned;
            existingProject.PositionClosed = projectUpdateDto.PositionClosed;
            existingProject.Location = projectUpdateDto.Location;
            existingProject.Stream = projectUpdateDto.Stream;
            existingProject.Level = projectUpdateDto.Level;
            existingProject.Priority = projectUpdateDto.Priority;
            existingProject.Attachment = projectUpdateDto.Attachment;

            _context.ProjectCandidates.RemoveRange(existingProject.ProjectCandidates);

            if (projectUpdateDto.CandidateIds != null)
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            _context.Projects.Remove(project);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }    
    }
}
