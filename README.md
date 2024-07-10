**Reservation API**

This project was created using C# WebApi. Due to the time constraint, I was unable to add unit tests.

Another trade-off was using an in-memory store for the reservations and appointments.
 In a real world application, there definitely would have been a SQL/NoSQL database to contain this data.

Handling timezones can definitely be a tricky issue. For the sake of the project and to reduce complexity everything was kept at UTC.

**General Approach/Summary**

+ Controllers have minimal logic while the services have majority of the business logic.
  + Split up into two different controllers; one for the provider and one for the client.
+ Helper class was introduced to move some of the logic out in order to help with readability and testability.
+ Models represent the different classes/objects the application primarily uses.
+ Majority of business logic within the ReservationService.
+ Appointment = Time slots that are available to the client.
+ Reservation = Time that is booked by a client.



