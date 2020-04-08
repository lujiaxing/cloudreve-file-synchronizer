using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Downloads
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? Status { get; set; }
        public int? Type { get; set; }
        public string Source { get; set; }
        public ulong? TotalSize { get; set; }
        public ulong? DownloadedSize { get; set; }
        public string GId { get; set; }
        public int? Speed { get; set; }
        public string Parent { get; set; }
        public string Attrs { get; set; }
        public string Error { get; set; }
        public string Dst { get; set; }
        public uint? UserId { get; set; }
        public uint? TaskId { get; set; }
    }
}
