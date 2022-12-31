#nullable disable
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebAPI.Data;
using MyPortfolioWebAPI.Models;
using Microsoft.AspNetCore.Cors;
using System.Text;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace MyPortfolioWebAPI.Controllers
{
    [EnableCors("AllowOrigion")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly MyPortfolioContext _context;

        public EmailsController(MyPortfolioContext context)
        {
            _context = context;
        }

        // GET: api/Emails
        
        
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emails>>> GetEmails()
        {
            
           
            
            return await _context.Emails.ToListAsync();
        }

        // GET: api/Emails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Emails>> GetEmail(long id)
        {
            var email = await _context.Emails.FindAsync(id);

            if (email == null)
            {
                return NotFound();
            }

            return email;
        }

        // PUT: api/Emails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmail(long id, Emails email)
        {
            if (id != email.ID)
            {
                return BadRequest();
            }

            _context.Entry(email).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailExists(id))
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

        // POST: api/Emails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpPost]
        public async Task<ActionResult<Emails>> PostEmail(Emails email)
        {
            _context.Emails.Add(email);
            await _context.SaveChangesAsync();

            try
            {
                StringBuilder template = new();
                template.AppendLine("from  " + email.EmailAddress);
                template.AppendLine("Name  " + email.Name);
                template.AppendLine(email.Message);

                var Mails = new MimeMessage();
                Mails.From.Add(MailboxAddress.Parse("gshadow057@gmail.com"));
                Mails.To.Add(MailboxAddress.Parse("thabisofakude40@gmail.com"));
                Mails.Subject = " Portfilio response ";
                Mails.Body=new TextPart(TextFormat.Text) { Text = template.ToString() };
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("gshadow057@gmail.com", "mvdtjsnsfqntdgqe");
                smtp.Send(Mails);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return CreatedAtAction(nameof(GetEmail), new { id = email.ID }, email);
        }

        // DELETE: api/Emails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmail(long id)
        {
            var email = await _context.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }

            _context.Emails.Remove(email);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmailExists(long id)
        {
            return _context.Emails.Any(e => e.ID == id);
        }
    }
}
