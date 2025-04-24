using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<SaleEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<SaleEntity>> GetAllAsync();
        Task AddAsync(SaleEntity sale);
        void Update(SaleEntity sale);
        Task<bool> DeleteAsync(Guid id);
        Task SaveChangesAsync();
        IQueryable<SaleEntity> Query();

    }

}
