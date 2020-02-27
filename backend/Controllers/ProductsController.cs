using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private IProductService _productService;
        private IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin,User")]

        [HttpGet("{q?}")]
        public IActionResult GetProducts(string q = "")
        {
            if (q == "undefined")
                q = "";
            var claims = User.Claims.Select(x =>
              new
              {
                  Type = x.Type,
                  Value = x.Value

              });
            var products = _productService.Get(q);
            return Ok(products);

        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        public IActionResult GetProduct(string id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // [Authorize(Roles = "Admin")]   
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult PostProduct([FromBody][Required]ProductDto productDto)
        {
            if (!ModelState.IsValid || productDto == null || string.IsNullOrEmpty(productDto.Name))
                return BadRequest(ModelState);
            var product=_mapper.Map<Product>(productDto);
            _productService.Add(product);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult PutProduct(string id, [FromBody]ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product=_mapper.Map<Product>(productDto);

            if (!_productService.Update(product)) return NotFound();
            return Ok(new { Status = "Product updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {
            if (!_productService.Delete(id)) return BadRequest();
            return Ok(new { Status = "Product deleted" });
        }
    }
}