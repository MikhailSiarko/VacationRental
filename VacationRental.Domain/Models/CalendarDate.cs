using System;
using System.Collections.Generic;

namespace VacationRental.Domain.Models
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public List<CalendarBooking> Bookings { get; set; }
        public List<CalendarPreparation> PreparationTimes { get; set; }
    }
}