using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Core.Domain.Entities.Base
{
    public abstract class EntityBase : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }

        public Guid? CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? UpdatedUser { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? DeletedUser { get; set; }
        public DateTime? DeletedOn { get; set; }



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