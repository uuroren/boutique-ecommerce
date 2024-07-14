using Boutique.API.Models;
using Boutique.Application.Dtos;
using Boutique.Application.Dtos.CategoryDtos;
using Boutique.Application.Services.CategoryServices;
using Boutique.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController:ControllerBase {
        private readonly ICategoryService _categoryService;
        private readonly AwsS3Service _awsS3Service;
        public CategoryController(ICategoryService categoryService,AwsS3Service awsS3Service) {
            _categoryService = categoryService;
            _awsS3Service = awsS3Service;
        }

        [Authorize]
        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories() {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(new BaseResponse { Result = categories });
        }

        [Authorize]
        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] string id) {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(new { result = category });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromForm] CreateCategoryDto categoryDto,IFormFile file = null) {
            if(file != null && file.Length > 0) {
                var fileName = file.FileName;
                var contentType = file.ContentType;

                using(var fileStream = file.OpenReadStream()) {
                    var link = await _awsS3Service.UploadFileAsync(fileStream,fileName,contentType);
                    categoryDto.ImageUrl = link;
                }
            }

            await _categoryService.CreateCategoryAsync(categoryDto);
            return Ok(new BaseResponse { Message = "Category added successfully",Result = categoryDto });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryDto categoryDto,IFormFile file = null) {
            if(file != null && file.Length > 0) {
                var fileName = file.FileName;
                var contentType = file.ContentType;

                using(var fileStream = file.OpenReadStream()) {
                    var link = await _awsS3Service.UploadFileAsync(fileStream,fileName,contentType);
                    categoryDto.ImageUrl = link;
                }
            }

            var updatedCategory = await _categoryService.UpdateCategoryAsync(categoryDto);
            return Ok(new BaseResponse { Message = "Category updated successfully!",Result = updatedCategory,Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("deactive-category/{id}")]
        public async Task<IActionResult> DeactiveCategory([FromRoute] UpdateCategoryDto categoryDto) {
            categoryDto.IsActive = false;
            var updatedCategory = await _categoryService.UpdateCategoryAsync(categoryDto);
            return Ok(new BaseResponse { Message = "Category successfully deactivated!",Result = updatedCategory,Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("actived-category/{id}")]
        public async Task<IActionResult> ActivedCategory([FromRoute] UpdateCategoryDto categoryDto) {
            categoryDto.IsActive = true;
            var updatedCategory = await _categoryService.UpdateCategoryAsync(categoryDto);
            return Ok(new BaseResponse { Message = "Category activated successfully!",Result = updatedCategory,Success = true });
        }
    }
}
