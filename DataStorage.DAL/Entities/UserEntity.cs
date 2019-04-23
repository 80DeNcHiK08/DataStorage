using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataStorage.DAL.Entities
{
    public class UserEntity : IdentityUser
    {
        public ICollection<UserDocument> UserDocuments { get; set; }
        public ICollection<DocumentEntity> Documents { get; set; }
        public int StorageSize { get; set; }
    }
}
