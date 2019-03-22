using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.DAL.Entities
{
    public class UserEntity : IdentityUser
    {
        [Key]
        public int UserId { get; set; }
        public string Uname { get; set; }
        public override string Email { get; set; }
        public string Password { get; set; }
        //public byte[] PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }
    }

    
}
