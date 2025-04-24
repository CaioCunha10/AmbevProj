using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Interfaces.Service
{
    public interface ISaleService
    {
        Task<IEnumerable<SaleReadDTO>> GetAllAsync();
        Task<SaleReadDTO> GetByIdAsync(Guid id);
        Task<SaleReadDTO> PostAsync(SalePostDTO dto);
        Task<SaleReadDTO> PutAsync(Guid id, SalePutDTO dto);
        Task<bool> CancelAsync(Guid id);
        Task<bool> CancelItemAsync(Guid saleId, Guid itemId, SaleItemCancelDTO dto);
        Task<IEnumerable<SaleReadDTO>> GetFilteredAsync(string? cliente, bool? cancelado, int page, int pageSize);

    }
}
