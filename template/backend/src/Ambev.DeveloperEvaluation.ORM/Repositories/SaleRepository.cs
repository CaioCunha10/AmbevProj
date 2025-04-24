using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        #region Listagem de Vendas
        public async Task<IEnumerable<SaleEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Sales.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar vendas: {ex.Message}");
                return Enumerable.Empty<SaleEntity>();
            }
        }

        public async Task<SaleEntity?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Sales.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar venda: {ex.Message}");
                return null;
            }
        }
        #endregion

        #region Criação de Venda
        public async Task AddAsync(SaleEntity sale)
        {
            try
            {
                await _context.Sales.AddAsync(sale);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar venda: {ex.Message}");
            }
        }
        #endregion

        #region Atualização de Venda
        public void Update(SaleEntity sale)
        {
            try
            {
                _context.Sales.Update(sale);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar venda: {ex.Message}");
            }
        }
        #endregion

        #region Exclusão de Venda
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var sale = await _context.Sales.FindAsync(id);
                if (sale == null)
                    return false;

                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar venda: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Persistência
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Alterações salvas com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar alterações: {ex.Message}");
            }
        }
        #endregion
        public IQueryable<SaleEntity> Query()
        {
            return _context.Sales.Include(s => s.Items); 
        }

    }
}
