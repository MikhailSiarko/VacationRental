using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : BaseController
    {
        private readonly ICalendarService _calendarService;
        private readonly IMapper _mapper;

        public CalendarController(ICalendarService calendarService, IMapper mapper)
        {
            _calendarService = calendarService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int rentalId, [FromQuery] DateTime start, [FromQuery] int nights)
        {
            var result = _calendarService.Get(rentalId, start, nights);

            return ProcessCreateResult(result, calendar => _mapper.Map<Calendar, CalendarViewModel>(calendar));
        }
    }
}