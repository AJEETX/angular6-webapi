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
        Product GetById(long id);
        Product Add(Product product);
        bool Update(Product product);
        bool Delete(long id);
    }
    public class ProductService : IProductService
    {
        readonly DataContext _context;
        Random random=new Random();
        public ProductService(IOptions<Settings> settings)
        {
            _context = new DataContext(settings);
        }
        public Product Add(Product product)
        {
            product.Time = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            product.PId=random.Next();
            _context.Products.InsertOne(product);
            return product;
        }

        public List<Product> Get(string q = "")
        {
            var products=default(List<Product>);
            try
            {
                products= _context.Products.Find(u => u.Name.Contains(q) || u.Location.Contains(q) || u.EventNo.Contains(q) )?.ToList();
            }
            catch (Exception)
            {
                // log or manage the exception
            }
            return products;
        }

        public Product GetById(long id)
        {
            var product=default(Product);
            try
            {
                product= _context.Products.Find(p => p.PId == id)?.FirstOrDefault();
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
                var filter = Builders<Product>.Filter.Eq(p => p.PId, productInfo.PId);
                var update = Builders<Product>.Update
                .Set(p => p.Name,productInfo.Name)
                .Set(p => p.EventNo, productInfo.EventNo)
                .Set(p => p.Location, productInfo.Location)
                .Set(p => p.Watch, productInfo.Watch)
                .Set(p => p.Detail, productInfo.Detail)
                .Set(p => p.Amountlost, productInfo.Amountlost);      
                var updateResult = _context.Products.UpdateOne(filter,update);

                return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
            }
            catch (Exception)
            {
                // log or manage the exception
                return false;
            }
        }
        public bool Delete(long id)
        {
            try
            {
                DeleteResult actionResult = _context.Products.DeleteOne(Builders<Product>.Filter.Eq("PId", id));

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