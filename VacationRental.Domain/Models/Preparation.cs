namespace VacationRental.Domain.Models
{
    public class Preparation : Occupation
    {
        public override OccupationType Type => OccupationType.Preparation;
    }
}