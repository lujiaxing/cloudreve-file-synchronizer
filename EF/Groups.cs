using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Groups
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Name { get; set; }
        public string Policies { get; set; }
        public ulong? MaxStorage { get; set; }
        public bool? ShareEnabled { get; set; }
        public bool? WebDavEnabled { get; set; }
        public int? SpeedLimit { get; set; }
        public string Options { get; set; }
    }
}
