using System;
using System.Collections.Generic;
using DataStorage.DAL.Entities;

namespace DataStorage.BLL.DTOs
{
    public class DocumentDTO
    {
        public string DocumentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ParentId { get; set; }
        public bool IsFile { get; set; }
        public string OwnerId { get; set; }
        public UserDTO Owner { get; set; }
        public long Length { get; set; }
        public string DocumentLink { get; set; }
        public bool IsPublic { get; set; }
        public DateTime ChangeDate { get; set; }
        public ICollection<UserDocument> UserDocuments { get; set; }

        public DocumentDTO()
        {
            UserDocuments = new List<UserDocument>();
        }
    }
}
