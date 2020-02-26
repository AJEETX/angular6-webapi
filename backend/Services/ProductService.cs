using System;
using WebApi.Helpers;
using WebApi.Model;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WebApi.Services
{
    public interface IProductService
    {
        List<Product> Get(string q);
        Product GetById(int id);
        Product Add(Product product);
        bool Update(Product product);
        bool Delete(int id);
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
            try
            {
                return _context.Products.Find(u => u.Name.Contains(q))?.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Product GetById(int id)
        {
            try
            {
                return _context.Products.Find(u => u.ID == id)?.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Update(Product productInfo)
        {
            try
            {
                var product = GetById(productInfo.ID);

                ReplaceOneResult actionResult = _context.Products
                                                .ReplaceOne(n => n.ID.Equals(productInfo.ID)
                                                                , product
                                                                , new ReplaceOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                DeleteResult actionResult = _context.Products.DeleteOne(
                     Builders<Product>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}