using System;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.DAL.Entities
{
    public class UserDocument
    {
        [Key]
        public Guid DocumentId { get; set; }
        public virtual DocumentEntity Document { get; set; }
        public bool EditAccess { get; set; }
        public bool WatchAccess { get; set; }
    }
}
