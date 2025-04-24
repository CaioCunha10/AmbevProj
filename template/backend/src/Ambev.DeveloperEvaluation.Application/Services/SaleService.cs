using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Interfaces.Service;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;      
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;
        private readonly ISaleItemRepository _itemRepository;

        public SaleService(
            ISaleRepository repository,
            ISaleItemRepository itemRepository,
            IMapper mapper,
            ILogger<SaleService> logger)
        {
            _repository = repository;
            _itemRepository = itemRepository;
            _mapper = mapper;
            _logger = logger;
        }


        #region Listagem de Vendas
        public async Task<IEnumerable<SaleReadDTO>> GetAllAsync()
        {
            var sales = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SaleReadDTO>>(sales);
        }

        public async Task<SaleReadDTO> GetByIdAsync(Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            return _mapper.Map<SaleReadDTO>(sale);
        }
        #endregion

        #region Criação de Venda
        public async Task<SaleReadDTO> PostAsync(SalePostDTO salePostDTO)
        {
            var saleEntity = _mapper.Map<SaleEntity>(salePostDTO);
            saleEntity.Id = Guid.NewGuid();
            saleEntity.SaleDate = DateTime.UtcNow;
            saleEntity.Cancelled = false;

            decimal totalAmount = 0;

            foreach (var item in saleEntity.Items)
            {
                item.Id = Guid.NewGuid();
                item.SaleId = saleEntity.Id;
                item.Cancelled = false;

                if(item.Quantity <= 0)
        throw new InvalidOperationException($"O item '{item.ProductName}' possui quantidade inválida (0 ou negativa).");

                if (item.UnitPrice <= 0)
                    throw new InvalidOperationException($"O item '{item.ProductName}' possui preço unitário inválido (0 ou negativo).");

                if (item.Quantity > 20)
                    throw new InvalidOperationException($"O item '{item.ProductName}' excede o limite de 20 unidades.");

                if (item.Quantity > 20)
                    throw new InvalidOperationException($"O item '{item.ProductName}' excede o limite de 20 unidades.");

                // Aplicando descontos
                if (item.Quantity >= 10 && item.Quantity <= 20)
                {
                    item.Discount = item.UnitPrice * item.Quantity * 0.2m;
                }
                else if (item.Quantity >= 4)
                {
                    item.Discount = item.UnitPrice * item.Quantity * 0.1m;
                }
                else
                {
                    item.Discount = 0;
                }

                item.Total = (item.UnitPrice * item.Quantity) - item.Discount;
                totalAmount += item.Total;
            }

            saleEntity.TotalAmount = totalAmount;

            await _repository.AddAsync(saleEntity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Venda criada com sucesso. ID: {SaleId}, Total: {Total}", saleEntity.Id, saleEntity.TotalAmount);

            return _mapper.Map<SaleReadDTO>(saleEntity);
        }
        #endregion

        #region Atualização de Venda
        public async Task<SaleReadDTO> PutAsync(Guid id, SalePutDTO salePutDTO)
        {
            var existingSale = await _repository.GetByIdAsync(id);
            if (existingSale == null)
                throw new Exception("Venda não encontrada");

            _mapper.Map(salePutDTO, existingSale);
            _repository.Update(existingSale);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Venda atualizada. ID: {SaleId}", existingSale.Id);

            return _mapper.Map<SaleReadDTO>(existingSale);
        }
        #endregion

        #region Cancelamento de Venda
        public async Task<bool> CancelAsync(Guid id)
        {
            var existingSale = await _repository.GetByIdAsync(id);
            if (existingSale == null)
                return false;

            if (existingSale.Cancelled)
                throw new InvalidOperationException("Esta venda já foi cancelada.");

            existingSale.Cancelled = true;
            _repository.Update(existingSale);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Venda cancelada. ID: {SaleId}", id);

            return true;
        }
        #endregion

        #region Cancelamento de Item
        public async Task<bool> CancelItemAsync(Guid saleId, Guid itemId, SaleItemCancelDTO dto)
        {
            var sale = await _repository.GetByIdAsync(saleId);
            if (sale == null)
                throw new KeyNotFoundException("Venda não encontrada.");

            if (sale.Cancelled)
                throw new InvalidOperationException("Não é possível cancelar item de uma venda já cancelada.");

            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item == null || item.SaleId != saleId)
                return false;

            if (item.Cancelled)
                throw new InvalidOperationException("Este item já foi cancelado.");

            item.Cancelled = true;
            _itemRepository.Update(item);

            // Atualiza o total da venda
            sale.TotalAmount = sale.Items
                .Where(i => i.Id != item.Id && !i.Cancelled)
                .Sum(i => i.Total);

            _repository.Update(sale);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Item cancelado. SaleID: {SaleId}, ItemID: {ItemId}", saleId, itemId);

            return true;
        }
        #endregion

        public async Task<IEnumerable<SaleReadDTO>> GetFilteredAsync(string? cliente, bool? cancelado, int page, int pageSize)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(cliente))
                query = query.Where(s => s.Customer.Contains(cliente));

            if (cancelado.HasValue)
                query = query.Where(s => s.Cancelled == cancelado.Value);

            var skip = (page - 1) * pageSize;
            var result = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SaleReadDTO>>(result);
        }

    }
}
