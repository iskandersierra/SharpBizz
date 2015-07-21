using System.Collections.Generic;
using Identity.Abstractions.Commands;
using Identity.Abstractions.Events;
using SharpBizz.Domain;

namespace Identity.Domain
{
    public class CreateIdentityScopeProcessor //: CreationCommandProcessor
    {
        public CreateIdentityScope Command { get; set; }

        //public override IEnumerable<IDomainEvent> GetEvents()
        //{
        //    yield return new IdentityScopeCreated { Name = Command.Name };
        //}
    }
}
