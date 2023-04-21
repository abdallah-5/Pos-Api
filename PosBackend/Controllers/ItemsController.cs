using PosBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PosBackend.Models.RequestDTO;
using PosBackend.Models.ResponseDTO;

namespace apiForAli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var responseModel = await _context.Products.Include(e => e.Sizes).Select(e => new AllItemsResponse
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Picture = Convert.ToBase64String(e.Picture!),
                Size = e.Sizes!.Count()
            }).ToListAsync();


            return Ok(responseModel);
        }



        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemRequestDTO createDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Products.AddAsync(new Product
            {
                Name = createDTO.Name,
                Price = createDTO.Price,
                Picture = Convert.FromBase64String(createDTO.Picture.Split(',')[1])
            });

            _context.SaveChanges();

            var currentProduct = await _context.Products.SingleOrDefaultAsync(e => e.Name == createDTO.Name);

            if (currentProduct != null)
            {
                foreach (var item in createDTO.Sizes)
                {
                    await _context.Sizes.AddAsync(new Size
                    {
                        Name = item.Name,
                        Price = item.Price,
                        ProductId = currentProduct.Id
                    });
                }
                _context.SaveChanges();
            }

            return Ok();
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateItem(int Id, UpdateItemRequestDTO updateDTO)
        {
            if (Id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(Id);

            if (product == null)
                return NotFound();

            
            var sizesRelated = await _context.Sizes.Where(e => e.ProductId == product.Id).ToListAsync();
            _context.Sizes.RemoveRange(sizesRelated);
            _context.SaveChanges();



            product.Name = updateDTO.Name;
            product.Price = updateDTO.Price;
            product.Picture = Convert.FromBase64String(updateDTO.Picture.Split(',')[1]);
            _context.SaveChanges();


        

            for (int i = 0; i < updateDTO.Sizes.Count; i++)
            {
                await _context.Sizes.AddAsync(new Size
                {
                    Name = updateDTO.Sizes[i].Name,
                    Price = updateDTO.Sizes[i].Price,
                    ProductId = Id
                });
                _context.SaveChanges();
            }


           
            return Ok();




        }




        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(Id);

            if (product == null)
                return NotFound();

            var responseModel = _context.Products.Include(e => e.Sizes).Where(e => e.Id == product.Id).Select(e => new GetItemByResponse
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Picture = Convert.ToBase64String(e.Picture!),
                Sizes = e.Sizes!.Select(e => new SizeModel
                {
                    Name = e.Name,
                    Price = e.Price,
                })



            }).SingleOrDefault();

           

            return Ok(responseModel);
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteItem([FromBody] int id)
        {
            if (id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            var productSizes = await _context.Sizes.Where(e => e.ProductId == product.Id).ToListAsync();

            if (productSizes != null)
            _context.Sizes.RemoveRange(productSizes);
            _context.Products.Remove(product);

            _context.SaveChanges();

            var test = "";

            return Ok();
        }





    }



}
