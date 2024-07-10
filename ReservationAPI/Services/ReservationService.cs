namespace ReservationAPI;

public class ReservationService : IReservationService
{
    // These private fields below are in memory and should ideally be replaced with an actual DB.
    private readonly List<Appointment> _appointmentSlots = new List<Appointment>();
    private readonly List<Reservation> _reservations = new List<Reservation>(); // Reservations made.

    private int _appointmentIds = 1;
    private int _reservationIds = 10;

    public async Task<List<Appointment>> GenerateAppointmentSlotsAsync(ProviderAvailability availability)
    {
        // This is a naive way of creating an appointment (auto incrementing)
        // Should have also had a way to generate a unique id and make sure the start time and end time wasn't already created.
        // E.g. There's no use of having two appointments with unique ids but the same start and end time
        // if the provider "accidentally" entered in the same availability.
        var slots = new List<Appointment>();
        var currentTime = availability.StartTime;

        while (currentTime < availability.EndTime)
        {
            slots.Add(new Appointment
            {
                Id = _appointmentIds++,
                Provider = availability.Provider,
                StartTime = currentTime,
                EndTime = currentTime.AddMinutes(15),
                IsAvailable = true
            });

            currentTime = currentTime.AddMinutes(15);
        }
        _appointmentSlots.AddRange(slots);
        return await Task.FromResult(_appointmentSlots);
    }

    public async Task<List<Appointment>> GetAvailableSlotsAsync(int? providerId = null)
    {
        return await Task.Run(() =>
        {
            // var allSlots = _appointmentSlots.Values.SelectMany(list => list);
            // Not the best way to refresh the stale reservations
            ReservationHelper.RefreshStaleReservations(_reservations, _appointmentSlots);
            if (providerId.HasValue)
            {
                return _appointmentSlots
                    .Where(s => s.Provider.Id == providerId.Value && s.IsAvailable)
                    .ToList();
            }
            else
            {
                return _appointmentSlots
                    .Where(s => s.IsAvailable)
                    .ToList();
            }
        });
    }

    public async Task<Reservation> ReserveSlotAsync(int slotId, int providerId, string clientName)
    {
        var slot = _appointmentSlots.FirstOrDefault(slot => slot.Id == slotId && slot.Provider.Id == providerId);

        ReservationHelper.ValidateAppointmentSlot(slot);
        ReservationHelper.ValidateReservationWindow(slot);

        slot.IsAvailable = false;

        var reservation = new Reservation
        {
            Id = _reservationIds++,
            AppointmentId = slotId,
            Appointment = slot,
            ClientName = clientName,
            Status = "Pending",
            TimeReservationBooked = DateTime.UtcNow
        };

        _reservations.Add(reservation);
        return await Task.FromResult(reservation);
    }

    public async Task<Reservation> ConfirmReservationAsync(int? reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
        {
            throw new InvalidOperationException("Reservation not found.");
        }

        reservation.Status = "Confirmed";
        return await Task.FromResult(reservation);
    }

}
