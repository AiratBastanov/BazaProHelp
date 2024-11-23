using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return user;
        }

        [HttpGet("email")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return NotFound();

            return user;
        }

        [HttpGet("checkemail")]
        public async Task<ActionResult<int>> GetEmailandCheck(string email, string nameOperation)
        {
            if (nameOperation == "register")
            {
                emailService = new EmailService();
                createVerificationCode = new CreateVerificationCode();
                VerificationCode = createVerificationCode.RandomInt(6);

                string bodyMessage = $"Проверочный код: {VerificationCode}";
                await emailService.SendEmailAsync(email, "GlutenApp", bodyMessage);
            }
            else
            {
                // Проверяем, есть ли пользователь с указанным email
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser == null)
                    return NotFound("Пользователь с таким email не найден.");
            }   
            return VerificationCode;
        }


        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
                return Conflict("Пользователь с таким email уже существует.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest(new { message = "Email и пароль не могут быть пустыми" });
            }

            try
            {
                // Находим пользователя по email
                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser == null)
                {
                    return NotFound(new { message = $"Пользователь с почтой {user.Email} не найден" });
                }

                // Заполняем остальные поля из найденного пользователя
                existingUser.PasswordHash = user.PasswordHash; // Обновляем только пароль, который передан в запросе
               /* existingUser.RoleId = existingUser.RoleId;*/

                // Помечаем пользователя как изменённого
                _context.Entry(existingUser).State = EntityState.Modified;

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();

                // Возвращаем обновлённого пользователя
                return Ok(existingUser); // Возвращаем обновлённого пользователя
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = "Ошибка при обновлении данных", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка на сервере", details = ex.Message });
            }
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
