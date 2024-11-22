using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAPIProHelp.Models;
using WebAppMacroSociety.EmailServies;
using WebAppMacroSociety.Randoms;

namespace WebAPIProHelp.Controllers
{
    [Route("glutenappapi/user")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly GlutenFreeAppContext _context;
        private CreateVerificationCode createVerificationCode;
        private EmailService emailService;
        private int VerificationCode = 0;

        public UsersController(GlutenFreeAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return user;
        }

        [HttpGet("checkemail")]
        public async Task<int> GetEmailandCheck(string email)
        {
            emailService = new EmailService();
            createVerificationCode = new CreateVerificationCode();
            VerificationCode = createVerificationCode.RandomInt(6);
            string bodyMessage = @"Проверочный код: " + VerificationCode.ToString();
            await emailService.SendEmailAsync(email, "GlutenApp", bodyMessage);
            return VerificationCode;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserId)
                return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.UserId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
