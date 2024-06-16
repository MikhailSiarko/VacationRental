using System;

namespace VacationRental.Domain.Models
{
    public abstract class Occupation
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int Unit { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public abstract OccupationType Type { get; }
    }
}