using System;
namespace Ambev.DeveloperEvaluation.Domain.Entities.Products;

public class Product
{
    public Guid Id { get; private set; }
    public string ProductCode { get; private set; } 
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public ProductCategory Category { get; private set; }

    protected Product() { }

    public static Product Create(string productCode, string name, decimal price, ProductCategory category)
    {
        if (price <= 0)
            throw new ArgumentException("Preço deve ser maior que zero");

        return new Product
        {
            Id = Guid.NewGuid(),
            ProductCode = productCode,
            Name = name,
            Price = price,
            Category = category,
            Stock = 0 
        };
    }
}