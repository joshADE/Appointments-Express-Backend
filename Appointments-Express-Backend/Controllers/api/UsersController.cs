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
using Appointments_Express_Backend.AuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace Appointments_Express_Backend.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppointmentDBContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public UsersController(AppointmentDBContext context, IMapper mapper, IUserService userService, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }


        // POST: api/Users/register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] User user)
        {
            user = _userService.Register(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.id }, _mapper.Map<UserResponse>(user));
        }

        // POST: api/Users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
        {
            var user = _userService.Login(loginRequest.username, loginRequest.password);
            if (user == null)
            {
                return NotFound(new LoginResponse { errors = "A user with those credentials was not found." });
            }
           
            return Ok(new LoginSuccessResponse { token = _jwtAuthenticationManager.Authenticate(user), user = _mapper.Map<UserResponse>(user) });
            
        } 

        // GET: api/Users
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            return _mapper.Map<List<UserResponse>>(await _context.Users.ToListAsync());
        }

        // GET: api/Users/loadUser
        [HttpGet("loadUser")]
        public async Task<ActionResult<UserResponse>> LoadUser()
        {
            var userIdString = Authorization.GetUserId(User);
            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });

            var user = _mapper.Map<UserResponse>(await _context.Users.FindAsync(int.Parse(userIdString)));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // GET: api/Users/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = _mapper.Map<UserResponse>(await _context.Users.FindAsync(id));

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
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }


        [HttpPost("appointmanager/{storeId}/{username}")]
        public async Task<IActionResult> AppointManager([FromRoute] int storeId, [FromRoute] string username)
        {
            var userIdString = Authorization.GetUserId(User);

            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });

            var ownerId = int.Parse(userIdString);
            var owner = await _context.Users.FindAsync(ownerId);
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            var store = await _context.Stores.FindAsync(storeId);

            if (owner == null) return NotFound(new { errors = "Owner not found" });
            if (manager == null) return NotFound(new { errors = "New manager not found" });
            if (store == null) return NotFound(new { errors = "Store not found" });
            if (!Authorization.UserHasPermission(_context, ownerId, storeId, "Assign Managers")) return Unauthorized(new { errors = "User not authorized to assign managers for this store" });
            if (await _context.UserStoreRoles.FirstOrDefaultAsync(usr => usr.userId == manager.id && usr.storeId == storeId) != null) return BadRequest(new { errors = "User already is assigned a role to that store" });
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.name == "Manager");
            if (role == null) return StatusCode(StatusCodes.Status500InternalServerError, new { errors = "There was an error in the DB" });


            _context.UserStoreRoles.Add(new UserStoreRole { userId = manager.id, storeId = storeId, roleId = role.id });
            await _context.SaveChangesAsync();

            return Ok();



        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
