
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosBackend.Models;
using PosBackend.Models.ResponseDTO;

namespace PosBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.Include(e => e.Sizes).ToListAsync();

            var responseModel = products.SelectMany(p => p.Sizes.Select(s => new AllProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name + " - " + s.Name,
                Price = s.Price,
                Picture = Convert.ToBase64String(p.Picture!),
                Size = s.Name,
                SizeId = s.Id,
            })).ToList();

            return Ok(responseModel);

        }


    }
}
