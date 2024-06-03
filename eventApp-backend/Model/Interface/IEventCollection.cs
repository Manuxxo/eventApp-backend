using Microsoft.AspNetCore.Mvc;

namespace eventApp_backend.Model.Interface
{
    public interface IEventCollection
    {
        Task<Event> Get(string id);
        Task<List<Event>> GetAllEvents();
        Task<List<Event>> GetLastEvents(int limit);
        Task Create(Event newEvent);
        Task Update(Event newEvent);
        Task Delete(string id);
        Task<List<Guest>> GetGuests(string eventId);
        Task AddGuest(string eventId, Guest newGuest);
        Task UpdateGuest(string eventId, Guest updatedGuest);
        Task DeleteGuest(string eventId, string guestId);
        Task AddAttendance(string  attendance);
    }
}
