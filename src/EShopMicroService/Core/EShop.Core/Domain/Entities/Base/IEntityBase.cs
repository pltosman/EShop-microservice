using System;

namespace EShop.Core.Domain.Entities.Base
{
    public interface IEntityBase
    {
    }

    public interface IEntityBase<out TKey> : IEntityBase where TKey : IEquatable<TKey>
    {
        public TKey Id { get; }
        DateTime CreatedAt { get; set; }
    }
}