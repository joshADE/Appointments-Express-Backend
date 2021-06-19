using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appointments_Express_Backend.Models;
using AutoMapper;
using Appointments_Express_Backend.DTO.Responses;
using Appointments_Express_Backend.Services;
using Appointments_Express_Backend.DTO.Requests;

namespace Appointments_Express_Backend.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppointmentDBContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(AppointmentDBContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }


        // POST: api/Users/register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            user = _userService.Register(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.id }, user);
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public LoginResponse Login([FromBody] LoginRequest loginRequest)
        {
            var user = _userService.Login(loginRequest.username, loginRequest.password);
            if (user == null)
            {
                return new LoginResponse { errors = "Invalid credentials" };
            }
           
            return new LoginSuccessResponse { token = "token here"/*JwtManager.GenerateToken(user.username)*/, user = _mapper.Map<UserResponse>(user) };
            
        } 

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
