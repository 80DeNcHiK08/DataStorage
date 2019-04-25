using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataStorage.DAL.Entities
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserDocument> UserDocuments { get; set; }
        // public virtual ICollection<DocumentEntity> Documents { get; set; }
        public long StorageSize { get; set; }
        public long RemainingStorageSize { get; set; }

        public UserEntity()
        {
            UserDocuments = new List<UserDocument>();
        }
    }
}
