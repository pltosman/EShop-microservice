using System;
using Products.EShop.Products.Entities;
using MongoDB.Driver;

namespace Products.EShop.Products.Data.Interfaces
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
