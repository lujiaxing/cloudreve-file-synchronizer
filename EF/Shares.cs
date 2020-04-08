using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Shares
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Password { get; set; }
        public bool? IsDir { get; set; }
        public uint? UserId { get; set; }
        public uint? SourceId { get; set; }
        public int? Views { get; set; }
        public int? Downloads { get; set; }
        public int? RemainDownloads { get; set; }
        public DateTime? Expires { get; set; }
        public bool? PreviewEnabled { get; set; }
        public string SourceName { get; set; }
    }
}
