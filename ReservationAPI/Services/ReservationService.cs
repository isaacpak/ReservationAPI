namespace ReservationAPI;

public class ReservationService : IReservationService
{
    // These private fields below are in memory and should ideally be replaced with an actual DB.

    private readonly Dictionary<int, List<Appointment>> _appointmentSlots = new Dictionary<int, List<Appointment>>(); // Available appointment slots
    private readonly List<Reservation> _reservations = new List<Reservation>(); // Reservations made.

    private int _appointmentIds = 1;
    private int _reservationIds = 10;

    public async Task<List<Appointment>> GenerateAppointmentSlotsAsync(ProviderAvailability availability)
    {
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
        _appointmentSlots[availability.Provider.Id] = [.. slots];
        return await Task.FromResult(_appointmentSlots[availability.Provider.Id]);
    }

    public async Task<List<Appointment>> GetAvailableSlotsAsync(int? providerId = null)
    {
        return await Task.Run(() =>
        {
            var allSlots = _appointmentSlots.Values.SelectMany(list => list);
            // Not the best way to refresh the stale reservations
            ReservationHelper.RefreshStaleReservations(_reservations, allSlots);
            if (providerId.HasValue)
            {
                return allSlots
                    .Where(s => s.Provider.Id == providerId.Value && s.IsAvailable)
                    .ToList();
            }
            else
            {
                return allSlots
                    .Where(s => s.IsAvailable)
                    .ToList();
            }
        });
    }

    public async Task<Reservation> ReserveSlotAsync(int slotId, int providerId, string clientName)
    {
        var slot = _appointmentSlots.Values
                                    .SelectMany(list => list)
                                    .FirstOrDefault(slot => slot.Id == slotId && slot.Provider.Id == providerId);

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
