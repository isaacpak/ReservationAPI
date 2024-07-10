using Microsoft.AspNetCore.Mvc;

namespace ReservationAPI;


[ApiController]
[Route("api/[controller]")]
public class ProviderController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ProviderController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddAvailability([FromBody] ProviderAvailability availability)
    {
        if (availability == null) { return BadRequest(); }
        var generatedSlots = await _reservationService.GenerateAppointmentSlotsAsync(availability);
        return Ok(generatedSlots);
    }

}
