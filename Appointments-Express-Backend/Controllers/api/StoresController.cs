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

namespace Appointments_Express_Backend.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly AppointmentDBContext _context;

        public StoresController(AppointmentDBContext context)
        {
            _context = context;
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


            var entity = await _context.Stores.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
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

            return Ok(entity);
        }

        // POST: api/Stores/createstore
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("createstore")]
        public async Task<ActionResult<Store>> CreateStore(Store store)
        {
            var userId = Authorization.GetUserId(User);
            if (userId == null)
                return NotFound();

            //if(!Authorization.UserHasPermission(_context, int.Parse(userId), store.id, "Create Store")){  // Create Store Permission Doesn't actually exist in DB
                //return Unauthorized();
            //}


            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Stores.Add(store);
                    await _context.SaveChangesAsync();

                    _context.UserStoreRoles.Add(new UserStoreRole { userId = int.Parse(userId), storeId = store.id, roleId = _context.Roles.FirstOrDefault(r => r.name == "Owner").id });
                    await _context.SaveChangesAsync();

                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("A database error has occured", ex);
                }
            }

            

            return CreatedAtAction("GetStore", new { id = store.id }, store);
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
