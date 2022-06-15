using System;

namespace ApplicationCore.Entity
{
    public class OTP : BaseEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string Code { get; set; }

    }
}