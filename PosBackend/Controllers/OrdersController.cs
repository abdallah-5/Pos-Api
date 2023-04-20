
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosBackend.Models;
using PosBackend.Models.RequestDTO;
using PosBackend.Models.ResponseDTO;

namespace PosBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var responseModel = await _context.Orders.Select(e => new AllOrdersResponseDTO
            {
                Id = e.Id,
                UserName = e.UserName,
                TotalPrice = e.TotalPrice,
                Phone = e.Phone,

            }).ToListAsync();



            return Ok(responseModel);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrderDetails(int Id)
        {
            if (Id == null)
                return BadRequest();

            var order = await _context.Orders.FindAsync(Id);

            if (order == null)
                return NotFound();
            var responseModel = await _context.OrderDetails.Where(e => e.Id == Id).Include(e => e.Product).ThenInclude(e => e.Sizes).Select(e => new OrderDetailsResponseDTO
            {

                OrderId = e.OrdereId,
                Name = e.Product.Name,
                Picture = Convert.ToBase64String(e.Product.Picture!),
                Price = e.Size.Price,
                SizeName = e.Size.Name,


                Count = e.Count,
                TotalItemPrice = e.TotalItemPrice,




            }).ToListAsync();

            return Ok(responseModel);


        }




        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderRequestDTO createDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           await _context.Orders.AddAsync(new Order
            {
                UserName = createDTO.UserName,
                Phone = createDTO.Phone,
                TotalPrice = createDTO.TotalPrice
            });
            _context.SaveChanges();

            var Curentorder = await _context.Orders.Where(e => e.UserName == createDTO.UserName).SingleOrDefaultAsync();
            
            var allProductInCart = await _context.ProductShoppingCarts.Include(e => e.Product).ThenInclude(e => e.Sizes).ToListAsync();

             
            for (int i = 0; i < allProductInCart.Count; i++)
            {
               
                await _context.OrderDetails.AddAsync(new OrderDetails
                {
                    ProductId = allProductInCart[i].ProductId,
                    SizeId = allProductInCart[i].SizeId,
                    Count = allProductInCart[i].Count,
                    TotalItemPrice = allProductInCart[i].TotalItemPrice,
                    OrdereId = Curentorder!.Id
                });
                _context.SaveChanges();
            }

            var ProductsInShoppingCart = await _context.ProductShoppingCarts.ToListAsync();
            _context.ProductShoppingCarts.RemoveRange(ProductsInShoppingCart);
            _context.SaveChanges();

            return Ok(new { isSUccess = true });
        }



    

    }
}
