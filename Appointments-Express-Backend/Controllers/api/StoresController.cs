using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appointments_Express_Backend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Appointments_Express_Backend.AuthenticationManager;
using Appointments_Express_Backend.DTO.Responses;
using AutoMapper;
using Appointments_Express_Backend.DTO.Requests;

namespace Appointments_Express_Backend.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly AppointmentDBContext _context;
        private readonly IMapper _mapper;

        public StoresController(AppointmentDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Stores
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            return await _context.Stores.ToListAsync();
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            return store;
        }

        // PUT: api/Stores/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            if (id != store.id)
            {
                return BadRequest();
            }

            var storeFromDB = await _context.Stores.FindAsync(id);

            if (storeFromDB == null)
            {
                return NotFound();
            }

            store.createdAt = storeFromDB.createdAt;
       
            _context.Entry(storeFromDB).State = EntityState.Detached;
            _context.Stores.Update(store);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
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


        // PATCH: api/Stores/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStore(int id, [FromBody] JsonPatchDocument<Store> jsonPatch)
        {
            var userIdString = Authorization.GetUserId(User);
            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });
            var userId = int.Parse(userIdString);

            var entity = await _context.Stores.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            if(!Authorization.UserHasPermission(_context, userId, entity.id, "Edit Store Details")){
                return Unauthorized();
            }

            jsonPatch.ApplyTo(entity);
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var storeResult = await FindUserStoreById(userId, entity.id);
            return Ok(storeResult);
        }

        // POST: api/Stores/createstore
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("createstore")]
        public async Task<ActionResult<Store>> CreateStore([FromBody] CreateStoreRequest createStoreRequest)
        {
            var userIdString = Authorization.GetUserId(User);
            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });

            //if(!Authorization.UserHasPermission(_context, int.Parse(userId), store.id, "Create Store")){  // Create Store Permission Doesn't actually exist in DB
            //return Unauthorized();
            //}
            var store = createStoreRequest.store;
            var userId = int.Parse(userIdString);
            if (store.isQuickProfile)
            {
                var userHasQuickProfile = await _context.UserStoreRoles.Include(usr => usr.store).AnyAsync(usr => usr.userId == userId && usr.store.isQuickProfile);
                // a user can only have one quick profile in the DB
                if (userHasQuickProfile)
                    return BadRequest(new { errors = "User only allowed one quick profile" });
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Stores.Add(store);
                    await _context.SaveChangesAsync();

                    _context.UserStoreRoles.Add(new UserStoreRole { userId = userId, storeId = store.id, roleId = _context.Roles.FirstOrDefault(r => r.name == "Owner").id });
                    await _context.SaveChangesAsync();

                    foreach (var storeHours in createStoreRequest.storeHours) {
                        storeHours.storeId = store.id;
                        _context.StoreHours.Add(storeHours);
                    }

                    foreach (var closed in createStoreRequest.closedDaysTimes)
                    {
                        closed.storeId = store.id;
                        _context.ClosedDaysTimes.Add(closed);
                    }
                    await _context.SaveChangesAsync();


                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("A database error has occured", ex);
                }
            }

            var fullStoreDetails = await FindUserStoreById(userId, store.id);

            return CreatedAtAction("GetUserStore", new { id = store.id }, fullStoreDetails);
        }

        [HttpGet("userstores")]
        public async Task<ActionResult<IEnumerable<UserStoreResponse>>> GetUserStores()
        {
            var userIdString = Authorization.GetUserId(User);
            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });
            var userId = int.Parse(userIdString);

            var listOfUserStoresWithRole = await _context.UserStoreRoles.Where(usr => usr.userId == userId).Include(usr => usr.store).Include(usr => usr.role).Select(usr =>
              new UserStoreResponse {
                  store = usr.store,
                  role = usr.role,
                  storeHours = _context.StoreHours.Where(sh => sh.storeId == usr.storeId).ToList(),
                  closedDaysTimes = _context.ClosedDaysTimes.Where(cdt => cdt.storeId == usr.storeId).ToList()
              }
           ).ToListAsync();

            return listOfUserStoresWithRole;
            
        }

        // GET: api/Stores/userstores/5
        [HttpGet("userstores/{id}")]
        public async Task<ActionResult<UserStoreResponse>> GetUserStore(int id)
        {
            var userIdString = Authorization.GetUserId(User);
            if (userIdString == null) return BadRequest(new { errors = "Invalid authenticated user" });
            var userId = int.Parse(userIdString);

            var userStoreWithRole = await FindUserStoreById(userId, id);

            if (userStoreWithRole == null)
            {
                return NotFound();
            }


            return userStoreWithRole;
        }

        private async Task<UserStoreResponse> FindUserStoreById(int userId, int storeId)
        {
            var userStoreWithRole = await _context.UserStoreRoles.Where(usr => usr.userId == userId && usr.storeId == storeId).Include(usr => usr.store).Include(usr => usr.role).Select(usr =>
              new UserStoreResponse
              {
                  store = usr.store,
                  role = usr.role,
                  storeHours = _context.StoreHours.Where(sh => sh.storeId == storeId).ToList(),
                  closedDaysTimes = _context.ClosedDaysTimes.Where(cdt => cdt.storeId == storeId).ToList()
              }
           ).FirstOrDefaultAsync();
            return userStoreWithRole;
        }

        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Store>> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return store;
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.id == id);
        }
    }
}
