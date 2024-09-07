using Microsoft.AspNetCore.Mvc;
using TJ.ProductManagement.Domain.Interfaces.Services;
using TJ.ProductManagement.Domain.Models;

namespace TJ.ProductManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService, INotificator notificator) : base(notificator)
        => _productService = productService;

        /// <summary>
        /// Return all products in database
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType<IEnumerable<ProductViewModel>>(200)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAll();
            return CustomResponse(result);
        }

        /// <summary>
        /// Return the product with the informed id
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType<ProductViewModel>(200)]
        [HttpGet("Id")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ValidateIdHexadecimal(id))
            {
                NotificateError("Invalid Id!");
                return CustomResponse();
            }
            var result = await _productService.GetById(id);
            return CustomResponse(result);
        }

        /// <summary>
        /// Add a new product in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Add(ProductInsertModel model)
        {
            await _productService.Add(model);
            return CustomResponse();
        }

        /// <summary>
        /// Update a product in database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(string id, ProductInsertModel model)
        {
            if (!ValidateIdHexadecimal(id))
            {
                NotificateError("Invalid Id!");
                return CustomResponse();
            }
            await _productService.Update(id, model);
            return CustomResponse();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Remove(string id)
        {
            if (!ValidateIdHexadecimal(id))
            {
                NotificateError("Invalid Id!");
                return CustomResponse();
            }
            await _productService.Delete(id);
            return CustomResponse();
        }
    }
}
