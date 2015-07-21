﻿using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/de-emphasize-scope", "0.1.0.0")]
    public class DeEmphasizeScope : IDomainCommand
    {
    }
}