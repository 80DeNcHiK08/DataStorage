using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataStorage.DAL.Entities
{
    public class UserEntity : IdentityUser
    {
        public virtual ICollection<UserDocument> UserDocuments { get; set; }
        public virtual ICollection<DocumentEntity> Documents { get; set; }
        public int StorageSize { get; set; }
    }
}
