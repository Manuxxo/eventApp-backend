namespace eventApp_backend.Model.Interface
{
    public interface IGuestCollection
    {
        Task<Guest> Get(string id);
        Task<List<Guest>> GetAllGuests();
        Task<string> Create(Guest newGuest);
        Task Update(Guest newGuest);
        Task Delete(string id);
    }
}
