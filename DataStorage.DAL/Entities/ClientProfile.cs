using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStorage.DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("UserEntity")]
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual UserEntity UserEntity { get; set; }
    }
}
