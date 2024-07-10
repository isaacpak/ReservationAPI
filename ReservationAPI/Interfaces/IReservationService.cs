namespace ReservationAPI;

public interface IReservationService
{
    Task<List<Appointment>> GenerateAppointmentSlotsAsync(ProviderAvailability availability);
    Task<List<Appointment>> GetAvailableSlotsAsync(int? providerId);
    Task<Reservation> ReserveSlotAsync(int slotId, int providerId, string clientName);
    Task<Reservation> ConfirmReservationAsync(int? reservationId);
}
