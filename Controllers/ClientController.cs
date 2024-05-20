using Cw6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Cw6.Controllers
{ 
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly S24515Context _context;

        public ClientController(S24515Context masterContext)
        {
            _context = masterContext;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients.FindAsync(idClient);

            if (client == null)
            {
                return NotFound();
            }

            var clientTripsCount = await _context.ClientTrips
                .CountAsync(ct => ct.IdClient == idClient);

            if (clientTripsCount > 0)
            {
                return BadRequest("Cannot remove client from database.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
