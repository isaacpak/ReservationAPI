namespace ReservationAPI;

public class Appointment
{
    public int Id { get; set; }
    public Provider Provider { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
}
