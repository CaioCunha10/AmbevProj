using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Infrastructure.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly DefaultContext _context;

        public SaleItemRepository(DefaultContext context)
        {
            _context = context;
        }

        #region Get
        public async Task<SaleItemEntity> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.SaleItems.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar item de venda por ID: {ex.Message}", ex);
            }
        }
        #endregion

        #region Update
        public void Update(SaleItemEntity item)
        {
            try
            {
                _context.SaleItems.Update(item);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar item de venda: {ex.Message}", ex);
            }
        }
        #endregion

        #region SaveChanges
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar alterações de item de venda: {ex.Message}", ex);
            }
        }
        #endregion
    }
}
