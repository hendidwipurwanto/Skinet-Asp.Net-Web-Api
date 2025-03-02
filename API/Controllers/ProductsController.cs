using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.products.ToListAsync();
           
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            return product;
        }

       
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult>  UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExist(id))
            {
                return BadRequest("Cannot update this product");
            }
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExist(int id)
        {
            return _context.products.Any(x => x.Id == id);
        }

             
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.products.FindAsync(id);
            if(product== null)
            {
                return NotFound();            
            }
            _context.products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
