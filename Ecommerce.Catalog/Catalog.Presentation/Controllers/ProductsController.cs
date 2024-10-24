using Catalog.Application.DTOs;
using Catalog.Application.DTOs.Conversions;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Responses;

namespace Catalog.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository  productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (products == null)
            {
                return NotFound("Product is not available!");
            }

            var(p, list) = ProductConversion.FromEntity(null!, products);
            //if(list == null || list.Count() == 0)
            //{
            //    return NotFound("Products not available!");
            //}
            return Ok(list);
        }
        [HttpGet("{productId:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int productId)
        {
           Product product = await productInterface.GetByIdAsync(productId);
            if(product == null)
            {
                return NotFound($"Product {productId} is not found!");
            }

            var (_product, _) = ProductConversion.FromEntity(product, null!);
            return Ok(_product);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> CreateProduct(ProductDto product)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> UpdateProduct(ProductDto product)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{productId:int}")]
        public async Task<ActionResult<ServiceResponse>> DeleteProduct(int productId)
        {
            Product product = await productInterface.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound($"Product {productId} is not found!");
            }

            //Convert to entity
            var response = await productInterface.DeleteAsync(product);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
