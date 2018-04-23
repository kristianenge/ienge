using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEnge.Database;
using IEnge.Database.Entities;
using IEnge.Models;
using IEnge.Service.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IEnge.Controllers.Api
{
    [Route("api/[controller]")]
    public class AutomationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtFactory _jwtFactory;
        private readonly DatabaseContext _context;
        private readonly ILogger<JwtFactory> _logger;

        public AutomationController(IConfiguration configuration, IJwtFactory jwtFactory, DatabaseContext context,
            ILogger<JwtFactory> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _jwtFactory = jwtFactory;
            _context = context;
        }

        [HttpGet("automationprojects")]
        public IEnumerable<AutomationProject> GetAutomationProjects()
        {

            return new List<AutomationProject>()
            {
                new AutomationProject(){Name = "project A",Desc = "desc a"},
                new AutomationProject(){Name = "project B", Desc =  "desc b"}
            };

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAutomationProject([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var automationProject = await _context.AutomationProjects.SingleOrDefaultAsync(m => m.Id == id);

            if (automationProject == null)
            {
                return NotFound();
            }



            return Ok(automationProject);
        }

        [HttpPost("projects")]
        public async Task<IActionResult> PostAutomationProject([FromBody] AutomationProject automationProject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AutomationProjects.Add(automationProject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAutomationProject", new { id = automationProject.Id }, automationProject);
        }
    }
}
