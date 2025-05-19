using System;

namespace ST10261874_CLDV6212_POE.Models
{
    public class FileModel
    {
        public string Name { get; set; }    
        public long Size { get; set; }
        public DateTimeOffset? LastModified { get; set; }   

        public string DisplaySize
        {
            get
            {
                if (Size >= 1024 * 1024)
                    return $"{Size / 1024 / 1024} MB";
                if (Size >= 1024)
                    return $"{Size / 1024} KB";
                return $"{Size} Bytes";
            }
        }

        public string FileName { get; set; }
    }
}
