using System;

namespace SubQuip.Common.CommonData
{
    public class FileDetails
    {
        public string FileId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string ContentType { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
