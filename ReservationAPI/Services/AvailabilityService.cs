namespace ReservationAPI;

public class AvailabilityService : IAvailabilityService
{
    private readonly List<ProviderAvailability> _providerAvailabilities = new List<ProviderAvailability>();

    public async Task AddAvailabilitesAsync(ProviderAvailability providerAvailability)
    {
        await Task.Run(() => _providerAvailabilities.Add(providerAvailability));
    }

    public async Task<List<ProviderAvailability>> GetAvailabilitiesByProviderAsync(int providerId)
    {
        return await Task.Run(() => _providerAvailabilities.Where(a => a.Provider.Id == providerId).ToList());
    }
}
