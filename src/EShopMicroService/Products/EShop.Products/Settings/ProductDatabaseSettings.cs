﻿using System;
namespace Products.EShop.Products.Settings
{
    public class ProductDatabaseSettings: IProductDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
