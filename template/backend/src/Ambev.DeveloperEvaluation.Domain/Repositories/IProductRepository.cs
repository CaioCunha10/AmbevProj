using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetAllAsync();
        Task<ProductEntity> GetByIdAsync(Guid id);
        Task AddAsync(ProductEntity product);
        Task UpdateAsync(ProductEntity product);
        Task<bool> DeleteAsync(Guid id);
        Task CreateAsync(ProductEntity entity);

    }
}
