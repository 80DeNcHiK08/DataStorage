using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.DAL.Entities
{
    public class DocumentEntity
    {
        [Key]
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        /*public string PublicName { get; set; }
        public string DocumentLink { get; set; }
        public bool IsPublic { get; set; }
        public DateTime ChangeDate { get; set; }
        public UserEntity Owner { get; set; }
        public int Size { get; set; }
        public bool IsFile { get; set; }
        public ICollection<DocumentEntity> Children { get; set; }*/
    }
}
