using AutoMapper;
using Boutique.Application.Dtos;
using Boutique.Application.Dtos.CategoryDtos;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.ExternalServices;
using Boutique.Infrastructure.Repositories.CategoryRepositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.CategoryServices
{
    public class CategoryService:ICategoryService {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly RedisCacheService _cacheService;

        public CategoryService(ICategoryRepository categoryRepository,IMapper mapper,RedisCacheService cacheService) {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto categoryDto) {
            var category = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.CreateCategoryAsync(category);
            await _cacheService.RemoveCacheValueAsync("categories");
        }

        public async Task DeleteCategoryAsync(string id) {
            await _categoryRepository.DeleteCategoryAsync(id);
            await _cacheService.RemoveCacheValueAsync("categories");
        }

        public async Task<List<ResultCategoryDto>> GetCategoriesAsync() {
            var cacheKey = "categories";
            var cachedCategories = await _cacheService.GetCacheValueAsync<List<ResultCategoryDto>>(cacheKey);

            if(cachedCategories != null) {
                return cachedCategories;
            }

            var categories = await _categoryRepository.GetCategoriesAsync();
            var categoriesDto = _mapper.Map<List<ResultCategoryDto>>(categories);

            await _cacheService.SetCacheValueAsync(cacheKey,categoriesDto,TimeSpan.FromHours(1));

            return categoriesDto;
        }

        public async Task<ResultCategoryDto> GetCategoryByIdAsync(string id) {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            return _mapper.Map<ResultCategoryDto>(category);
        }

        public async Task<ResultCategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto) {
            var category = await _categoryRepository.GetCategoryByIdAsync(updateCategoryDto.CategoryId);
            if(category == null) {
                throw new KeyNotFoundException("Kategori bulunamadı.");
            }

            if(!string.IsNullOrEmpty(category.ImageUrl) && string.IsNullOrEmpty(updateCategoryDto.ImageUrl)) {
                updateCategoryDto.ImageUrl = category.ImageUrl;
            }

            _mapper.Map(updateCategoryDto,category);

            await _categoryRepository.UpdateCategoryAsync(category);
            await _cacheService.RemoveCacheValueAsync("categories");

            return _mapper.Map<ResultCategoryDto>(category);
        }
    }
}
