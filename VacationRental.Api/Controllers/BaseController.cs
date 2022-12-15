using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Domain;

namespace VacationRental.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult ProcessCreateResult<T, TK>(Result<T> result, Func<T, TK> okResponseModelBuilder)
        {
            if (result.HasErrors)
            {
                if (result.IsValidationFailure)
                {
                    return BadRequest(result.Errors);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }

            return Ok(okResponseModelBuilder(result.Value));
        }

        protected IActionResult ProcessGetResult<T, TK>(Result<T> result, string entityName,
            Func<T, TK> okResponseModelBuilder)
        {
            if (result.HasErrors)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);

            if (result.IsEmpty)
                return NotFound(new {Message = $"{entityName} not found"});

            return Ok(okResponseModelBuilder(result.Value));
        }
    }
}