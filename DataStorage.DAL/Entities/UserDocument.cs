using System;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.DAL.Entities
{
    public class UserDocument
    {
        [Key]
        public string DocumentId { get; set; }
        public DocumentEntity Document { get; set; }
        public string GuestEmail { get; set; }
        public string DocumentLink { get; set; }
        // public bool EditAccess { get; set; }
    }
}
