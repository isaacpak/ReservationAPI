namespace ReservationAPI;

public class ProviderAvailability
{
    public int Id { get; set; }
    public Provider Provider { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
