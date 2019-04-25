using System;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.DAL.Entities
{
    public class UserDocument
    {
        public string DocumentId { get; set; }
        public DocumentEntity Document { get; set; }
        
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        // public string GuestEmail { get; set; }
        // public string DocumentLink { get; set; }
    }
}
