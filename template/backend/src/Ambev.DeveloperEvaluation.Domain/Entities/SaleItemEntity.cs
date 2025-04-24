using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItemEntity
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public string? ProductName { get; set; } 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        public SaleEntity Sale { get; set; }
        public bool Cancelled { get; set; }
    }

}
