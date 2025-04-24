using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;


public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;
    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        => await _context.Products.ToListAsync();

    public async Task<ProductEntity> GetByIdAsync(Guid id)
        => await _context.Products.FindAsync(id);

    public async Task AddAsync(ProductEntity product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductEntity product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity == null)
            return false;

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task CreateAsync(ProductEntity entity)
    {
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

}
