using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities.Base
{
    public abstract class EntityBase : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get;  set; }


        /// <summary>
        /// If you want to clone your order Entity just call it 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public EntityBase Clone()
        {
            return (EntityBase)this.MemberwiseClone();
        }
    }
}