namespace DataStorage.DAL.Entities
{
    public class UserDocument
    {
        public virtual DocumentEntity Document { get; set; }
        public bool EditAccess { get; set; }
        public bool WatchAccess { get; set; }
    }
}
