using System;
using Payment.Domain.Entities.Base;

namespace Payment.Domain.Entities
{
    public class Card : EntityBase
    {     
        public string LongNum { get; set; }
       
        public string Expires { get; set; }
       
        public string CCV { get; set; }
    }
}
