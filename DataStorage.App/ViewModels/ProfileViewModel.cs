using System.ComponentModel.DataAnnotations;

namespace DataStorage.App.ViewModels
{
    public class ProfileViewModel
    {
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserId { get; set; }
        
        public string Email { get; set; }

        public string Token { get; set; }

        public long StorageSize { get; set; }
    }
}
