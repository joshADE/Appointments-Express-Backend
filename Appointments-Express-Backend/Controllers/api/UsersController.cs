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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.JsonPatch;

namespace Appointments_Express_Backend.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private const string Tags = "backend_User_Photo";
        private readonly AppointmentDBContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly Cloudinary _cloudinary;

        public UsersController(AppointmentDBContext context, IMapper mapper, IUserService userService, IJwtAuthenticationManager jwtAuthenticationManager, Cloudinary cloudinary)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _cloudinary = cloudinary;
        }


        // POST: api/Users/register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromForm] RegisterRequest request)
        {
            User user = _mapper.Map<User>(request);
            user = _userService.Register(user);

            // taken from https://github.com/cloudinary/CloudinaryDotNet/blob/master/samples/PhotoAlbum/Pages/Upload.cshtml.cs
            if (request.avatar != null)
            {
                var result = await _cloudinary.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(request.avatar.FileName, request.avatar.OpenReadStream()),
                    Tags = Tags
                }).ConfigureAwait(false);

                user.avatarPublicId = result.PublicId;
                user.avatarUrl = result.SecureUrl.AbsoluteUri;
            }
            //user.createdAt = DateTime.Now;
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


        [HttpPost("appoint/{role}/{storeId}/{username}")]
        public async Task<IActionResult> AppointRole([FromRoute] string role, [FromRoute] int storeId, [FromRoute] string username)
        {
            var userIdString = Authorization.GetUserId(User);

            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });

            var roleFromDB = await _context.Roles.FirstOrDefaultAsync(r => r.name == role);
            if (roleFromDB == null) return NotFound(new { errors = "Role not found" });

            var userId = int.Parse(userIdString);
            var appointer = await _context.Users.FindAsync(userId);
            var appointee = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            var store = await _context.Stores.FindAsync(storeId);

            if (appointer == null) return NotFound(new { errors = "User not found" });
            if (appointee == null) return NotFound(new { errors = $"New {role} not found" });
            if (store == null) return NotFound(new { errors = "Store not found" });
            if (!Authorization.UserHasPermission(_context, userId, storeId, $"Assign {role}s")) return Unauthorized(new { errors = $"User not authorized to assign {role} for this store" });
            if (await _context.Stores.FirstOrDefaultAsync(s => s.id == storeId && s.isQuickProfile) != null) return BadRequest(new { errors = $"Cannot assign {role} role to that store (quick profile)" });
            if (await _context.UserStoreRoles.FirstOrDefaultAsync(usr => usr.userId == appointee.id && usr.storeId == storeId) != null) return BadRequest(new { errors = "User already is assigned a role to that store" });
            


            _context.UserStoreRoles.Add(new UserStoreRole { userId = appointee.id, storeId = storeId, roleId = roleFromDB.id });
            await _context.SaveChangesAsync();

            return Ok();



        }

        [HttpPost("unappoint/{role}/{storeId}/{username}")]
        public async Task<IActionResult> UnappointRole([FromRoute] string role, [FromRoute] int storeId, [FromRoute] string username)
        {
            var userIdString = Authorization.GetUserId(User);

            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });

            var roleFromDB = await _context.Roles.FirstOrDefaultAsync(r => r.name == role);
            if (roleFromDB == null) return NotFound(new { errors = "Role not found" });

            var userId = int.Parse(userIdString);
            var appointer = await _context.Users.FindAsync(userId);
            var appointee = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            var store = await _context.Stores.FindAsync(storeId);

            if (appointer == null) return NotFound(new { errors = "User not found" });
            if (appointee == null) return NotFound(new { errors = $"Appointed {role} not found" });
            if (store == null) return NotFound(new { errors = "Store not found" });
            if (!Authorization.UserHasPermission(_context, userId, storeId, $"Assign {role}s")) return Unauthorized(new { errors = $"User not authorized to un assign {role} role for this store" });
            if (await _context.Stores.FirstOrDefaultAsync(s => s.id == storeId && s.isQuickProfile) != null) return BadRequest(new { errors = $"Cannot de assign {role} role to that store (quick profile)" });

            var userStoreRole = await _context.UserStoreRoles.FirstOrDefaultAsync(usr => usr.userId == appointee.id && usr.storeId == storeId && usr.roleId == roleFromDB.id);

            if (userStoreRole == null) return BadRequest(new { errors = $"User not assigned {role} role to that store" });


            _context.UserStoreRoles.Remove(userStoreRole);
            await _context.SaveChangesAsync();

            return Ok();



        }

        [HttpPatch("editaccount")]
        public async Task<IActionResult> EditAccount([FromBody] JsonPatchDocument<EditAccountRequest> jsonPatch)
        {
            var userIdString = Authorization.GetUserId(User);
            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });
            var userId = int.Parse(userIdString);

            var userDatabase = await _context.Users.FindAsync(userId);

            if (userDatabase == null)
            {
                return NotFound();
            }

            EditAccountRequest userDTO = _mapper.Map<EditAccountRequest>(userDatabase);

            jsonPatch.ApplyTo(userDTO);

            _mapper.Map(userDTO, userDatabase);

            userDatabase = _userService.EditAccount(userDatabase);
            _context.Entry(userDatabase).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Ok(_mapper.Map<UserResponse>(userDatabase));
        }

        [AllowAnonymous]
        [HttpGet("usersandrolesforstore/{storeId}")]
        public async Task<ActionResult<IEnumerable<UserAndRoleForStoreResponse>>> GetUsersAndRolesForStore([FromRoute] int storeId)
        {
            var usersAndRoles = _context.UserStoreRoles.Where(usr => usr.storeId == storeId)
                .Include(usr => usr.role)
                .Include(usr => usr.user)
                .Include(usr => usr.store).Select(usr => new UserAndRoleForStoreResponse
                {
                    user = _mapper.Map<UserResponse>(usr.user),
                    createdAt = usr.createdAt,
                    store = usr.store,
                    role = usr.role
                });

            
            return await usersAndRoles.ToListAsync();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
