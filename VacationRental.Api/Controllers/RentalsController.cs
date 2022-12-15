using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : BaseController
    {
        private readonly IRentalService _rentalService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalService rentalService, IMapper mapper)
        {
            _rentalService = rentalService;
            _mapper = mapper;
        }

        [HttpGet("{rentalId:int}")]
        public IActionResult Get(int rentalId)
        {
            var result = _rentalService.Get(rentalId);

            return ProcessGetResult(result, nameof(Rental), rental => _mapper.Map<Rental, RentalViewModel>(rental));
        }

        [HttpPost]
        public IActionResult Post(RentalBindingModel model)
        {
            var result = _rentalService.Create(model.Units, model.PreparationTimeInDays);

            return ProcessCreateResult(result, rental => _mapper.Map<Rental, ResourceIdViewModel>(rental));
        }

        [HttpPut("{rentalId:int}")]
        public IActionResult Post(int rentalId, RentalBindingModel model)
        {
            var result = _rentalService.Update(rentalId, model.Units, model.PreparationTimeInDays);

            return ProcessCreateResult(result, rental => _mapper.Map<Rental, ResourceIdViewModel>(rental));
        }
    }
}