using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Files
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Name { get; set; }
        public string SourceName { get; set; }
        public uint? UserId { get; set; }
        public ulong? Size { get; set; }
        public string PicInfo { get; set; }
        public uint? FolderId { get; set; }
        public uint? PolicyId { get; set; }
    }
}
