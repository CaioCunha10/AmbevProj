using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleItemRepository
    {
        Task<SaleItemEntity> GetByIdAsync(Guid id);
        void Update(SaleItemEntity item);
        Task SaveChangesAsync();
    }
}
