using Cw6.Models;
using Cw6.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Cw6.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripController : ControllerBase
    {
        private readonly S24515Context _context;


        public TripController(S24515Context masterContext)
        {
            _context = masterContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrips()
        {
            

            List<TripDTO> trips = await _context.Trips.Select(e => new TripDTO
            {
                IdTrip = e.IdTrip,
                Name = e.Name,
                Description = e.Description,
                DateFrom = e.DateFrom,
                DateTo = e.DateTo,
                MaxPeople = e.MaxPeople,
                Clients = e.ClientTrips.Select(x => new ClientTripDTO {
                    FirstName = x.IdClientNavigation.FirstName,
                    LastName = x.IdClientNavigation.LastName
                }).ToList(),
                Countries = e.IdCountries.Select(x => new CountryTripDTO
                {
                    Name = x.Name
                }).ToList()

            }).OrderBy(e=>e.DateFrom).ToListAsync();

           



            return Ok(trips);
        }
      
        [HttpPost ("{IdTrip}/clients")]

        public async Task<IActionResult> AssignClientToTrip(int IdTrip, AssignClientDTO assignClientDTO)
        {
            if (IdTrip != assignClientDTO.IdTrip)
            {
                return BadRequest("IdTrip doesn't match query.");
            }

            var trip = await _context.Trips
                .Include(t => t.ClientTrips)
                .Select(t => new
                {
                    IdTrip = t.IdTrip,
                    Name = t.Name,
                    ClientTrips = t.ClientTrips.Select(ct => new
                    {
                        IdClient = ct.IdClient
                    })
                })
                .FirstOrDefaultAsync(t => t.IdTrip == assignClientDTO.IdTrip && t.Name == assignClientDTO.TripName);

            if (trip == null)
            {
                return BadRequest("Trip doesn't exist.");
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == assignClientDTO.Pesel);

            if (client != null)
            {
                if (trip.ClientTrips.Any(ct => ct.IdClient == client.IdClient))
                {
                    return BadRequest("The client is already assigned to this tour.");
                }
            }
            else
            {
                var newIdClient = await _context.Clients.MaxAsync(c => c.IdClient) + 1;

                client = new Client
                {
                    IdClient = newIdClient,
                    FirstName = assignClientDTO.FirstName,
                    LastName = assignClientDTO.LastName,
                    Pesel = assignClientDTO.Pesel,
                    Email = assignClientDTO.Email,
                    Telephone = assignClientDTO.Telephone
                };

                _context.Clients.Add(client);
            }

            var newClientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = IdTrip,
                PaymentDate = DateTime.Parse(assignClientDTO.PaymentDate),
                RegisteredAt = DateTime.Now
            };

            _context.ClientTrips.Add(newClientTrip);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
