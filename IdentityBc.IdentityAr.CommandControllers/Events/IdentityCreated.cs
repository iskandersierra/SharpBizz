using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityBc.IdentityAr.CommandControllers.Commands;

namespace IdentityBc.IdentityAr.CommandControllers.Events
{
    public abstract class IdentityEvent
    {
        [Key, Required, Column(Order = 1)]
        public string id { get; set; }
    }
    public class IdentityCreated : IdentityEvent
    {
        [Required]
        public string uri { get; set; }
    }
    public class IdentityAccepted : IdentityEvent
    {
    }
    public class IdentityRejected : IdentityEvent
    {
        public int reasonCode { get; set; }
        public string reason { get; set; }
    }
    public class IdentityForgotten : IdentityEvent
    {
    }
    
    public abstract class IdentityClaimEvent : IdentityEvent
    {
        [Key, Required, Column(Order = 2)]
        public string claimId { get; set; }
    }
    public class IdentityClaimCreated : IdentityClaimEvent
    {
        [Required]
        public string claimTypeId { get; set; }
        [Required]
        public string claimValue { get; set; }

        public List<ClaimProperty> ClaimProperties { get; set; }
    }
    public class IdentityClaimAccepted : IdentityClaimEvent
    {
    }
    public class IdentityClaimRejected : IdentityClaimEvent
    {
        public int reasonCode { get; set; }
        public string reason { get; set; }
    }
    public class IdentityClaimForgotten : IdentityEvent
    {
    }

}