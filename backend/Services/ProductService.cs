using System;
using WebApi.Helpers;
using WebApi.Model;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace WebApi.Services
{
    public interface IProductService
    {
        List<Product> Get(string q);
        Product GetById(string id);
        Product Add(Product product);
        bool Update(Product product);
        bool Delete(string id);
    }
    public class ProductService : IProductService
    {
        readonly DataContext _context;
        public ProductService(IOptions<Settings> settings)
        {
            _context = new DataContext(settings);
        }
        public Product Add(Product product)
        {
            product.Time = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            _context.Products.InsertOne(product);
            return product;
        }

        public List<Product> Get(string q = "")
        {
            var products=default(List<Product>);
            try
            {
                products= _context.Products.Find(u => u.Name.Contains(q))?.ToList();
            }
            catch (Exception)
            {
                // log or manage the exception
            }
            return products;
        }

        public Product GetById(string id)
        {
            var product=default(Product);
            try
            {
                product= _context.Products.Find(p => p.ID == GetInternalId(id) || p.Id==id )?.FirstOrDefault();
            }
            catch (Exception)
            {
                // log or manage the exception
            }
            return product;
        }
        // Try to convert the Id to a BSonId value
        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
        public bool Update(Product productInfo)
        {
            try
            {
                var updateResult = _context.Products.ReplaceOne(n => n.ID.Equals(productInfo.ID), productInfo, new ReplaceOptions { IsUpsert = true });
                return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
            }
            catch (Exception)
            {
                // log or manage the exception
                return false;
            }
        }
        public bool Delete(string id)
        {
            try
            {
                DeleteResult actionResult = _context.Products.DeleteOne(Builders<Product>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception)
            {
                // log or manage the exception
                return false;
            }
        }
    }
}