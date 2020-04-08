using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Policies
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Server { get; set; }
        public string BucketName { get; set; }
        public bool? IsPrivate { get; set; }
        public string BaseUrl { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public ulong? MaxSize { get; set; }
        public bool? AutoRename { get; set; }
        public string DirNameRule { get; set; }
        public string FileNameRule { get; set; }
        public bool? IsOriginLinkEnable { get; set; }
        public string Options { get; set; }
    }
}
