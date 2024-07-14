using Boutique.Application.Dtos;
using Boutique.Application.Dtos.CategoryDtos;
using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.CategoryServices {
    public interface ICategoryService {
        Task CreateCategoryAsync(CreateCategoryDto categoryDto);
        Task DeleteCategoryAsync(string id);
        Task<ResultCategoryDto> GetCategoryByIdAsync(string id);
        Task<List<ResultCategoryDto>> GetCategoriesAsync();
        Task<ResultCategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);

    }
}
