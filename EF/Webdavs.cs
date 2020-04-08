using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Webdavs
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public uint? UserId { get; set; }
        public string Root { get; set; }
    }
}
