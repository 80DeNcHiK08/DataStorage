using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.DAL.Entities
{
    public class DocumentEntity
    {
        // [Key]
        public string DocumentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ParentId { get; set; }
        public bool IsFile { get; set; }
        public string OwnerId { get; set; }
        // public UserEntity Owner { get; set; }
        public long Length { get; set; }
        public string DocumentLink { get; set; }
        public bool IsPublic { get; set; }
        public DateTime ChangeDate { get; set; }
        public ICollection<UserDocument> UserDocuments { get; set; }

        public DocumentEntity()
        {
            UserDocuments = new List<UserDocument>();
        }
    }
}
