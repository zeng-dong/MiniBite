using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBite.Api.Inventory.DataAccess;
using System.Threading.Tasks;

namespace MiniBite.Api.Inventory.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/inventory")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly InventoryDbContext _context;

        public ProductsController(InventoryDbContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public async Task<ActionResult> GetAll()
        {
            var all = await _context.Proudcts.ToListAsync();
            return Ok(all);
        }
    }
}
