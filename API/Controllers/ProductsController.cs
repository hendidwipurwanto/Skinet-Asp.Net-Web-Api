using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> _repo) : ControllerBase
    {
        
      
        // GET api/<ProductsController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            return product;
        }

       
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _repo.Add(product);
            if(await _repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product");
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult>  UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExist(id))
            {
                return BadRequest("Cannot update this product");
            }
            _repo.Update(product);

            if(await _repo.SaveAllAsync())
            {
                return NoContent();
            }
           

            return BadRequest("Problem updating the product");
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            //TODO: Implement method later
            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {

            //TODO: Implement method later
            return Ok();
        }

        private bool ProductExist(int id)
        {
            return _repo.Exist(id);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type);
            var producs = await _repo.ListAsync(spec);

            return Ok(producs); 
        }
             
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if(product== null)
            {
                return NotFound();            
            }
            _repo.Remove(product);
           if(await _repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the product");
        }
    }
}
