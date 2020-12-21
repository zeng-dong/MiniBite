using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBite.Api.Purchasing.DataAccess;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBite.Api.Purchasing.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/purchasing")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        readonly PurchasingDbContext _context;

        public ItemsController(PurchasingDbContext context, PurchasingDbInitializer initializer)
        {
            _context = context;

            if (!_context.Items.Any())
            {
                initializer.Seed(_context);
            }
        }

        [HttpGet("items")]
        public async Task<ActionResult> GetAll()
        {
            var all = await _context.Items.ToListAsync();
            return Ok(all);
        }
    }
}
