using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Tasks
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? Status { get; set; }
        public int? Type { get; set; }
        public uint? UserId { get; set; }
        public int? Progress { get; set; }
        public string Error { get; set; }
        public string Props { get; set; }
    }
}
