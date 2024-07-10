using System.Diagnostics.CodeAnalysis;

namespace ReservationAPI;

public static class ReservationHelper
{

    public static void ValidateAppointmentSlot(Appointment? slot)
    {
        if (slot == null || !slot.IsAvailable)
        {
            throw new InvalidOperationException("Slot is either not found or already booked.");
        }
    }

    public static void ValidateReservationWindow(Appointment slot)
    {
        var currentUtcDateTime = DateTime.UtcNow;
        var validReservationWindow = currentUtcDateTime.AddHours(24);
        if (slot.StartTime < validReservationWindow)
        {
            throw new InvalidOperationException("Reservations must be made more than 24 hours in advance.");
        }
    }

    public static void RefreshStaleReservations(List<Reservation> reservations, IEnumerable<Appointment> appointments)
    {
        var currentTime = DateTime.UtcNow;
        // Constants should be defined elsewhere
        var expiredReservations = reservations.Where(reservation => reservation.Status == "Pending" && (currentTime - reservation.TimeReservationBooked).TotalMinutes > 30)
                                              .ToList();

        foreach (var reservation in expiredReservations)
        {
            var appointmentSlot = appointments.FirstOrDefault(a => a.Id == reservation.AppointmentId);
            if (appointmentSlot != null)
            {
                appointmentSlot.IsAvailable = true;
            }
            reservations.Remove(reservation);
        }
    }
}
