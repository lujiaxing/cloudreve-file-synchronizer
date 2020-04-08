using System;
using System.Collections.Generic;

namespace Cloudreve_FileSynchronizer.EF
{
    public partial class Users
    {
        public uint Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public int? Status { get; set; }
        public uint? GroupId { get; set; }
        public ulong? Storage { get; set; }
        public string TwoFactor { get; set; }
        public string Avatar { get; set; }
        public string Options { get; set; }
        public string Authn { get; set; }
    }
}
