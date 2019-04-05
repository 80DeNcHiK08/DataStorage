using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataStorage.DAL.Entities
{
    public class UserEntity : IdentityUser
    {
        public virtual ICollection<UserDocument> UserDocuments { get; set; }
        public int StorageSize { get; set; }
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
