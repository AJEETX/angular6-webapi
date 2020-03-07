using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
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
            if (q == "undefined")  q = "";
            List<Product> products=default(List<Product>) ;
            try{
                var claims = User.Claims.Select(x => new {Type = x.Type, Value = x.Value});
                products = _productService.Get(q);
            }
            catch(AppException){
                
            }
            return Ok(products);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        public IActionResult GetProduct(long id)
        {
            Product product=default(Product);
            try{
                product = _productService.GetById(id);
                if (product == null) return NotFound();
            }
            catch(AppException){

            }
            return Ok(product);
        }

        // [Authorize(Roles = "Admin")]   
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult PostProduct([FromBody][Required]ProductDto productDto)
        {
            if (!ModelState.IsValid || productDto == null || string.IsNullOrEmpty(productDto.Name)) return BadRequest(ModelState);
            Product product=default(Product);
            try{
                product=_mapper.Map<Product>(productDto);
                product=_productService.Add(product);
            }
            catch(AppException){

            }
            return Ok(new {Product=product,StatusCode="Vehicle added"});
        }

        [HttpPut("{id}")]
        public IActionResult PutProduct(string id, [FromBody]ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Product product=default(Product);
            try{
                product=_mapper.Map<Product>(productDto);

            if (!_productService.Update(product)) return NotFound();
            }
            catch(AppException) {

            }
            return Ok(new { Status = "Vehicle updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(long id)
        {
            try{
                if (!_productService.Delete(id)) return BadRequest();
            }
            catch(AppException){

            }
            return Ok(new { Status = "Vehicle deleted" });
        }
    }
}