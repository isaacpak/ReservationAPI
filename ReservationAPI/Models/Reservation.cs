namespace ReservationAPI;

public class Reservation
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
    public string ClientName { get; set; }
    public string Status { get; set; }
    public DateTime TimeReservationBooked { get; set; }
}
