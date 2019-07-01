using System;

namespace Authentication.Models
{
    public class SkinnyProfile : Outcome
    {
        public Guid AccountId { get; set; }
        public Guid CrmId { get; set; }
        public Guid CommerceId { get; set; }
        public AccountStatus Status { get; set; }
    }
}
