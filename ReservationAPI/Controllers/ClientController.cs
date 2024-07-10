using Microsoft.AspNetCore.Mvc;

namespace ReservationAPI;


[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IReservationService _reservationService; // references the same instance in memory
    public ClientController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet("AvailableTimes")]
    public async Task<ActionResult<List<Appointment>>> GetAvailableTimeSlots([FromQuery] int? providerId = null)
    {
        // If query param doesn't have a specific doctor, return all available.
        // Otherwise, return specific doctor's schedule
        var availableTimeSlots = await _reservationService.GetAvailableSlotsAsync(providerId);
        return Ok(availableTimeSlots);
    }


    [HttpPost("Reserve")]
    public async Task<ActionResult<Reservation>> CreateClientReservation([FromBody] ClientReservationRequest clientReservationRequest)
    {
        try
        {
            if (clientReservationRequest == null) { return BadRequest(); }
            var reservedAppointmentSlot = await _reservationService.ReserveSlotAsync(clientReservationRequest.AppointmentId, clientReservationRequest.ProviderId, clientReservationRequest.ClientName);
            return Ok(reservedAppointmentSlot);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch("Confirm")]
    public async Task<ActionResult<Reservation>> ConfirmReservation([FromBody] int? reservationId)
    {
        try
        {
            if (reservationId == null) { return BadRequest(); }
            var confirmation = await _reservationService.ConfirmReservationAsync(reservationId.Value);
            return Ok(confirmation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }

}
