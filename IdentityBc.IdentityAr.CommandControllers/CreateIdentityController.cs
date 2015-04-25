using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityBc.IdentityAr.CommandControllers.Aggregates;
using IdentityBc.IdentityAr.CommandControllers.Commands;
using IdentityBc.IdentityAr.CommandControllers.Events;
using SharpBizz.Framework.EventSourcing;
using SharpBizz.WebApi;
using SharpBizz.WebApi.Results;

namespace IdentityBc.IdentityAr.CommandControllers
{
    public class CreateIdentityController : CommandController
    {
        public async Task<EmitEventsResult> Post(CreateIdentity command)
        {
            return EmitCreationEvents(new IdentityCreated
            {
                id = command.id, 
                uri = command.uri,
            });
        }
    }

    public class AcceptIdentityController : CommandController
    {
        public IAggregateSource AggregateSource { get; set; }
        public async Task<IHttpActionResult> Post(AcceptIdentity command)
        {
            var identityStatus = await IdentityStatusAggregate.LoadAsync(AggregateSource, command.id);

            if (identityStatus.IsForgotten)
                return BadRequest("Identity is forgotten and cannot be accepted");

            if (identityStatus.IsAccepted)
                return EmitNoEvents();

            return EmitCreationEvents(new IdentityAccepted
            {
                id = command.id,
            });
        }
    }

    public class RejectIdentityController : CommandController
    {
        public IAggregateSource AggregateSource { get; set; }
        public async Task<IHttpActionResult> Post(RejectIdentity command)
        {
            var identityStatus = await IdentityStatusAggregate.LoadAsync(AggregateSource, command.id);

            if (identityStatus.IsForgotten)
                return BadRequest("Identity is forgotten and cannot be rejected");

            if (!identityStatus.IsAccepted)
                return EmitNoEvents();

            return EmitCreationEvents(new IdentityRejected
            {
                id = command.id, 
                reasonCode = command.reasonCode, 
                reason = command.reason,
            });
        }
    }

    public class ForgetIdentityController : CommandController
    {
        public IAggregateSource AggregateSource { get; set; }
        public async Task<IHttpActionResult> Post(ForgetIdentity command)
        {
            var identityStatus = await IdentityStatusAggregate.LoadAsync(AggregateSource, command.id);

            if (identityStatus.IsForgotten)
                return EmitNoEvents();

            return EmitCreationEvents(new IdentityForgotten
            {
                id = command.id, 
            });
        }
    }

    public class CreateIdentityClaimController : CommandController
    {
        public IAggregateSource AggregateSource { get; set; }
        public async Task<IHttpActionResult> Post(CreateIdentityClaim command)
        {
            var aggregate = await CreateIdentityClaimAggregate.LoadAsync(AggregateSource, command.id, command.claimId);

            if (aggregate.IsIdentityForgotten)
                return BadRequest("Identity is forgotten and cannot be rejected");

            if (aggregate.IsClaimForgotten)
                return EmitNoEvents();

            return EmitCreationEvents(new IdentityForgotten
            {
                id = command.id, 
            });
        }
    }
}
