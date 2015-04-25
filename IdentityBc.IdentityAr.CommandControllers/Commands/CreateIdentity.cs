using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityBc.IdentityAr.CommandControllers.Commands
{
    public abstract class IdentityCommand
    {
        [Key, Required, Column(Order = 1)]
        public string id { get; set; }
        
    }
    public class CreateIdentity : IdentityCommand
    {
        [Required]
        public string uri { get; set; }
    }
    public class AcceptIdentity : IdentityCommand
    {
    }
    public class RejectIdentity : IdentityCommand
    {
        public int reasonCode { get; set; }
        public string reason { get; set; }
    }
    public class ForgetIdentity : IdentityCommand
    {
    }

    public abstract class IdentityClaimCommand : IdentityCommand
    {
        [Key, Required, Column(Order = 2)]
        public string claimId { get; set; }
    }
    public class CreateIdentityClaim : IdentityClaimCommand
    {
        [Required]
        public string claimTypeId { get; set; }
        [Required]
        public string claimValue { get; set; }

        public List<ClaimProperty> ClaimProperties { get; set; }
    }

    public abstract class ClaimProperty
    {
        [Required]
        public string name { get; set; }
    }

    /// <summary>
    /// https://tools.ietf.org/html/rfc6350#section-5.1
    /// </summary>
    public class ClaimCultureProperty : ClaimProperty
    {
        [Required]
        public string cultureName { get; set; }
    }
    /// <summary>
    /// https://tools.ietf.org/html/rfc6350#section-5.6
    /// </summary>
    public class ClaimSubtypeProperty : ClaimProperty
    {
        [Required]
        public string subtype { get; set; }
    }

    public class AcceptIdentityClaim : IdentityClaimCommand
    {
    }
    public class RejectIdentityClaim : IdentityClaimCommand
    {
        public int reasonCode { get; set; }
        public string reason { get; set; }
    }
    public class ForgetIdentityClaim : IdentityClaimCommand
    {
    }
}