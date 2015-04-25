using System;
using System.Threading.Tasks;
using IdentityBc.IdentityAr.CommandControllers.Events;
using SharpBizz.Framework.EventSourcing;

namespace IdentityBc.IdentityAr.CommandControllers.Aggregates
{
    public class IdentityStatusAggregate
    {
        public bool IsAccepted { get; set; }
        public bool IsForgotten { get; set; }

        public static Task<IdentityStatusAggregate> LoadAsync(IAggregateSource source, string id)
        {
            if (source == null) throw new ArgumentNullException("source");

            return source
                .SetupAggregate<IdentityStatusAggregate>(id)
                .When<IdentityCreated>(a => a.IsForgotten = false)
                .When<IdentityAccepted>(a => a.IsAccepted = true)
                .When<IdentityRejected>(a => a.IsAccepted = false)
                .When<IdentityForgotten>(a => a.IsForgotten = true)
                .LoadAsync();
        }
    }

    public class CreateIdentityClaimAggregate
    {
        public bool IsIdentityForgotten { get; set; }
        public bool IsClaimForgotten { get; set; }

        public static Task<CreateIdentityClaimAggregate> LoadAsync(IAggregateSource source, string id, string claimId)
        {
            if (source == null) throw new ArgumentNullException("source");

            return source
                .SetupAggregate<CreateIdentityClaimAggregate>(id)
                .When<IdentityCreated>(a => a.IsIdentityForgotten = false)
                .When<IdentityForgotten>(a => a.IsIdentityForgotten = true)
                .When<IdentityClaimCreated>(a => a.IsClaimForgotten = true).If((a, e) => e.claimId == claimId)
                .LoadAsync();
        }
    }
}