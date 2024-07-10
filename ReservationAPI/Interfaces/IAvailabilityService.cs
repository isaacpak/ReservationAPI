namespace ReservationAPI;

public interface IAvailabilityService
{
    Task AddAvailabilitesAsync(ProviderAvailability providerAvailability);
    Task<List<ProviderAvailability>> GetAvailabilitiesByProviderAsync(int providerId);
}
