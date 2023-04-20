
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosBackend.Models;
using PosBackend.Models.RequestDTO;
using PosBackend.Models.ResponseDTO;
using System.Linq;

namespace PosBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase

    {

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartRequestDTO createDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var ProductShoppingCart = _context.ProductShoppingCarts.SingleOrDefault(e => e.ProductId == createDTO.ProductId && e.SizeId == createDTO.SizeId);
            var Size = _context.Sizes.SingleOrDefault(e => e.Id == createDTO.SizeId);
            var Product = _context.Products.SingleOrDefault(e => e.Id == createDTO.ProductId);

            if (ProductShoppingCart == null)

            {
                await _context.ProductShoppingCarts.AddAsync(new ProductShoppingCart
                {
                    ProductId = createDTO.ProductId,
                    SizeId = createDTO.SizeId,
                    TotalItemPrice = Size.Price,

                });

                _context.SaveChanges();




                return Ok();

            }

            ProductShoppingCart.Count += 1;
            ProductShoppingCart.TotalItemPrice = Size.Price * ProductShoppingCart.Count;
            _context.SaveChanges();


            return Ok();

        }


        [HttpGet]

       

        public async Task<IActionResult> GetAll()
        {
            var responseModel = await _context.ProductShoppingCarts.Include(e => e.Product).ThenInclude(e => e.Sizes).Select(e => new AllproductsInCartResponseDTO
            {
                Id = e.Id,
                ProductId = e.ProductId,
                Name = e.Product.Name + " - " + e.Size.Name,
                Picture = Convert.ToBase64String(e.Product.Picture!),
                Price = e.Size.Price,
                Count = e.Count,
                TotalItemPrice = e.TotalItemPrice
            }).ToListAsync();

            var OrderTotal = _context.ProductShoppingCarts.Select(x => x.TotalItemPrice).Sum();
            return Ok(new { responseModel, OrderTotal });
        }

        [HttpPost]

        [Route("Delete")]
        public async Task<IActionResult> DeleteItem([FromBody]int id)
        {
            if (id == null)
                return BadRequest();

            var ProductShoppingCart = await _context.ProductShoppingCarts.FindAsync(id);

            if (ProductShoppingCart == null)
                return NotFound();


            _context.ProductShoppingCarts.Remove(ProductShoppingCart);

            _context.SaveChanges();

            return Ok();
        }




    }
}
