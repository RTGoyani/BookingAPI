using BookingAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingApiService _bookingApiService;

        public BookingController(BookingApiService bookingApiService)
        {
            _bookingApiService = bookingApiService;
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            var token = await _bookingApiService.GenerateTokenAsync();
            return Ok(new { Token = token });
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            var token = await _bookingApiService.GenerateTokenAsync();
            var cities = await _bookingApiService.GetCitiesAsync(token);
            return Ok(cities);
        }
    }
}
