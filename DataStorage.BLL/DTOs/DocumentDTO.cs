using System;

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
        public long Length { get; set; }
        public string DocumentLink { get; set; }
        // public bool IsPublic { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
