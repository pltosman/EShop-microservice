using System;
using EShop.Products.Entities;
using MongoDB.Driver;

namespace EShop.Products.Data.Interfaces
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
