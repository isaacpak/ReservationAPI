using Microsoft.AspNetCore.Mvc;

namespace ReservationAPI;


[ApiController]
[Route("api/[controller]")]
public class ProviderController : ControllerBase
{
    private readonly IAvailabilityService _availabilityService;
    private readonly IReservationService _reservationService;

    public ProviderController(IAvailabilityService availabilityService, IReservationService reservationService)
    {
        _availabilityService = availabilityService;
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddAvailability([FromBody] ProviderAvailability availability)
    {
        if (availability == null) { return BadRequest(); }
        await _availabilityService.AddAvailabilitesAsync(availability);
        var generatedSlots = await _reservationService.GenerateAppointmentSlotsAsync(availability);
        return Ok(generatedSlots);
    }

}
