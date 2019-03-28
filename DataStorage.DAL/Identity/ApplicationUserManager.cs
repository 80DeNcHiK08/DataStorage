using Microsoft.AspNetCore.Identity;

using DataStorage.DAL.Entities;

namespace DataStorage.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
        { }
    }
}
