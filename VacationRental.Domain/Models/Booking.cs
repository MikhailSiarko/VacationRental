namespace VacationRental.Domain.Models
{
    public class Booking : Occupation
    {
        public override OccupationType Type => OccupationType.Booking;
    }
}