using Microsoft.AspNetCore.Identity;

namespace DataStorage.BLL.DTOs
{
    public class UserDTO : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long StorageSize { get; set; }
        public long RemainingStorageSize { get; set; }
    }
}
