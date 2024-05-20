namespace Cw6.Models.DTOs
{
    public class TripDTO
    {
        public int IdTrip { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int MaxPeople { get; set; }

        public List<ClientTripDTO> Clients { get; set; } = [];

        public List<CountryTripDTO> Countries { get; set; } = [];
    }
}
