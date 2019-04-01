using Microsoft.AspNetCore.Identity;

namespace DataStorage.DAL.Entities
{
    public class UserEntity : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
