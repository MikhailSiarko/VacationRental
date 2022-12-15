using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet("{bookingId:int}")]
        public IActionResult Get(int bookingId)
        {
            var result = _bookingService.Get(bookingId);

            return ProcessGetResult(result, nameof(Booking),
                booking => _mapper.Map<Booking, BookingViewModel>(booking));
        }

        [HttpPost]
        public IActionResult Post(BookingBindingModel model)
        {
            var result = _bookingService.Create(model.RentalId, model.Start, model.Nights);

            return ProcessCreateResult(result, booking => _mapper.Map<Booking, ResourceIdViewModel>(booking));
        }
    }
}