using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Interfaces.Service;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Microsoft.Extensions.Logging;



namespace Ambev.DeveloperEvaluation.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, IMapper mapper, ILogger<ProductService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
    {
        _logger.LogInformation("Buscando todos os produtos...");
        var products = await _repository.GetAllAsync();
        _logger.LogInformation("Total de produtos encontrados: {Count}", products.Count());
        return _mapper.Map<IEnumerable<ProductReadDto>>(products);
    }

    public async Task<ProductReadDto> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando produto com ID: {ProductId}", id);
        var product = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductReadDto>(product);
    }

    public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto)
    {
        var product = _mapper.Map<ProductEntity>(dto);
        product.Id = Guid.NewGuid();

        await _repository.CreateAsync(product);
        _logger.LogInformation("Produto criado com sucesso. ID: {ProductId}", product.Id);

        return _mapper.Map<ProductReadDto>(product);
    }

    public async Task<ProductReadDto> UpdateAsync(Guid id, ProductUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Tentativa de atualizar produto inexistente. ID: {ProductId}", id);
            throw new Exception("Produto não encontrado.");
        }

        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);
        _logger.LogInformation("Produto atualizado. ID: {ProductId}", id);

        return _mapper.Map<ProductReadDto>(existing);
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Tentativa de excluir produto inexistente. ID: {ProductId}", id);
            throw new Exception("Produto não encontrado.");
        }

        await _repository.DeleteAsync(id);
        _logger.LogInformation("Produto excluído. ID: {ProductId}", id);
        return true;
    }

}
