using System;
using Ordering.Domain.Entities.Base;

namespace Ordering.Domain.Entities
{
    public class ClientRequest : EntityBase
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}