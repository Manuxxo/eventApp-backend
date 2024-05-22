using Microsoft.AspNetCore.Mvc;

namespace eventApp_backend.Model.Interface
{
    public interface IEventCollection
    {
        Task<Event> Get(string id);
        Task<List<Event>> GetAllEvents();
        Task Create(Event newEvent);
        Task Update(Event newEvent);
        Task Delete(string id);

    }
}
