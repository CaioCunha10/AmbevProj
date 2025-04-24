namespace Ambev.DeveloperEvaluation.Domain.Entities.Products
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;

        public static ProductEntity Create(string productCode, string name, decimal price, string category)
        {
            return new ProductEntity
            {
                Id = Guid.NewGuid(),
                ProductCode = productCode,
                Name = name,
                Price = price,
                Category = category,
                Stock = 0
            };
        }
     

    public void Update(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

    }    
}
