using System;
using System.Collections.Generic;
using System.Linq;
using DB;
using WebApi.Model;

namespace  WebApi.Services
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
    public ProductService(DataContext context)
    {
        _context=context;
    }
    public Product Add(Product product)
    {
        product.Time=DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
        _context.Product.Add(product);
        _context.SaveChanges();
        return product;
    }

    public bool Delete(int id)
    {
        var product = _context.Product.Find(id);
        if (product != null)
        {
            _context.Product.Remove(product);
            return _context.SaveChanges()!=0?true:false;
        }
        return false;
    }

    public List<Product> Get(string q="")
    {
        var allProduct = _context.Product.AsQueryable();
        if (!string.IsNullOrEmpty(q))
        {
            q = q.Trim().ToLowerInvariant();
            allProduct = allProduct.Where(m => m.Name.ToLowerInvariant().Contains(q)
                || m.Detail.ToLowerInvariant().Contains(q));
        }

        return allProduct.ToList();
    }

    public Product GetById(int id)
    {
        return _context.Product.Find(id);
    }

    public bool Update(Product product)
    {
        var update = _context.Product.Update(product);
        _context.SaveChanges();
        return update is null ? false : true;
    }
}
}