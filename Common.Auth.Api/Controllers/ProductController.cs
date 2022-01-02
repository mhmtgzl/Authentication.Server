using Common.Auth.Core.DTO;
using Common.Auth.Core.Models;
using Common.Auth.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {

        private readonly IService<Product,ProductDto> productService;

        public ProductController(IService<Product, ProductDto> service)
        {
            this.productService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultIntance(await productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto product)
        {
            return ActionResultIntance(await productService.AddAsync(product));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto product)
        {
            return ActionResultIntance(await productService.UpdateAsync(product,product.Id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultIntance(await productService.Remove(id));
        }
    }
}
