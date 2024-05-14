namespace eventApp_backend.Model
{
    public class EventManagementDatabaseSettings : IEventManagementDatabaseSettings
    {
        public string EventsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IEventManagementDatabaseSettings
    {
        string EventsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
