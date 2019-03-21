using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DataStorage.DAL.Models
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        //public string Name { get; set; }
        //public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string Phone { get; set; }
        //public DateTime BirthDate { get; set; }
        //public string Status { get; set; }
    }

    
}
