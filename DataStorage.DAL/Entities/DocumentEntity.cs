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
        public string Path { get; set; }
        public Guid ParentId { get; set; }
        public bool IsFile { get; set; }
        public Guid OwnerId { get; set; }
        public long Length { get; set; }
        /*public string DocumentLink { get; set; }
        public bool IsPublic { get; set; }
        public DateTime ChangeDate { get; set; }*/
    }
}
