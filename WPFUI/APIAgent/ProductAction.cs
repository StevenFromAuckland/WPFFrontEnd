﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HillLabTestEntities;

namespace WPFUI
{
    public class ProductAction : EntityAction<Product>, IProductAction
    {
        public ProductAction(HttpClient clientAPI)
            : base(clientAPI, "Products")
        {
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            IEnumerable<Product> products = await GetAll();
            if (products != null)
            {
                if (products.Count() > 0)
                {
                    ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                    IEnumerable<Category> categories = await wrapper.Category.GetAllCategories();
                    products.ToList().ForEach(x => x.Category = categories.FirstOrDefault(y => y.CategoryId == x.CategoryId));
                }
                return products.OrderBy(x => x.ProductName);
            }

            return null;
        }
        public async Task<Product> GetProductById(int productId)
        {
            Product p = await GetById(productId);
            if(p != null)
            {
                ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                Category category = await wrapper.Category.GetCategoryById(p.CategoryId);
                p.Category = category;
            }
            return p;
        }
        public async Task<Uri> CreateProduct(Product product)
        {
            return await Create(product);
        }
        public async Task<HttpStatusCode> UpdateProduct(Product product)
        {
            return await Update(product);
        }
        public async Task<HttpStatusCode> DeleteProduct(int productId)
        {
            return await Delete(productId);
        }

    }
}