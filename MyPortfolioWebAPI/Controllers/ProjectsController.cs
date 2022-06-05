#nullable disable
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using MyPortfolioWebAPI.Data;
using MyPortfolioWebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Cors;

namespace MyPortfolioWebAPI.Controllers
{
    [EnableCors("AllowOrigion")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly MyPortfolioContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
      
        public ProjectsController(MyPortfolioContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjects()
        {
            return await _context.Projects
                .Select(x=>new Projects() { 
                ID=x.ID,
                ProjectName=x.ProjectName,
                GithubLink=x.GithubLink,
                URLLink=x.URLLink,
                ImageName=x.ImageName,
                Message=x.Message,
                ImageSrc= String.Format("{0}://{1}{2}/Images/{3}",Request.Scheme,Request.Host,Request.PathBase,x.ImageName)
                })
                .ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Projects>> GetProjects(long id)
        {
            var projects = await _context.Projects.FindAsync(id);

            if (projects == null)
            {
                return NotFound();
            }

            return projects;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjects(long id, Projects projects)
        {
            if (id != projects.ID)
            {
                return BadRequest();
            }

            _context.Entry(projects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Projects>> PostProjects([FromForm]Projects projects)
        {
           
            projects.ImageName = await SaveImage(projects.ImageFile);
            _context.Projects.Add(projects);
            await _context.SaveChangesAsync();
           


           
          
            return CreatedAtAction("GetProjects", new { id = projects.ID }, projects);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjects(long id)
        {
            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }
            DeleteImage(projects.ImageName);
            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectsExists(long id)
        {
            return _context.Projects.Any(e => e.ID == id);
        }
        [NonAction]
        public  async Task<string> SaveImage(IFormFile ImageFile)
        {
            string ImageName = new String(Path.GetFileNameWithoutExtension(ImageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            ImageName = ImageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(ImageFile.FileName);
            var ImagePath= Path.Combine(_hostEnvironment.ContentRootPath,"Images" ,ImageName);
            using(var fileStream =new FileStream(ImagePath,FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }
            return ImageName;
        }
        [NonAction]
        public void DeleteImage(string imageName)
        {
            var ImagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }
        }
    }
}
