namespace ReservationAPI;

public class ClientReservationRequest
{
    public int AppointmentId { get; set; }
    public int ProviderId { get; set; }
    public string ClientName { get; set; }

}
