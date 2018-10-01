using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers
{
   [Route("products")]
    public class ProductsController : Controller
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(Roles = "Admin,User")]   

        [HttpGet("{q?}")]
        public IActionResult GetProducts(string q = "")
        {
            if(q=="undefined")
            q="";
            var claims=User.Claims.Select(x=>
            new {
                Type=x.Type,
                Value=x.Value

            });
            var products = _productService.Get(q);
            return Ok(products);

        }

        [Authorize(Roles = "Admin,User")]   
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // [Authorize(Roles = "Admin")]   
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult PostProduct([FromBody][Required]Product product)
        {
            if (!ModelState.IsValid || product == null || string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Detail) )
            return BadRequest(ModelState);
            _productService.Add(product);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody]Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            product.ID = id;
            if (!_productService.Update(product)) return NotFound();
            return Ok(new {Status="Product updated"});
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (!_productService.Delete(id)) return BadRequest();
            return Ok(new {Status= "Product deleted"});
        }
    }
}