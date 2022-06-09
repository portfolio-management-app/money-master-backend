using System.Collections.Generic;

namespace ApplicationCore.Entity.Utilities
{
    public class UserMobileFcmCode : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FcmCode { get; set; }
    }
}