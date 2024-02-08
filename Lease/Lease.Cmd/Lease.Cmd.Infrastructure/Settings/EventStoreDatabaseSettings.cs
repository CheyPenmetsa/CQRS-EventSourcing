namespace Lease.Cmd.Infrastructure.Settings
{
    public class EventStoreDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }
    }
}
