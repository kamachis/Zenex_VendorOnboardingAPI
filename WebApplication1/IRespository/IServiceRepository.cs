namespace Zenex.Registration.IRespository
{
    public interface IServiceRepository
    {
        Task SendReminderToInitializedVendor();
        Task SendReminderToSavedVendor();
        Task SendReminderToRegisteredVendor();
    }
}
