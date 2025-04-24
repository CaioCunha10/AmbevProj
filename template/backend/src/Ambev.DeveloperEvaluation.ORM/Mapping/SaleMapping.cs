using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleMap : IEntityTypeConfiguration<SaleEntity>
    {
        public void Configure(EntityTypeBuilder<SaleEntity> builder)
        {
            builder.ToTable("Sales");  
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SaleNumber).IsRequired();
            builder.Property(x => x.SaleDate).IsRequired();
            builder.Property(x => x.Customer).HasMaxLength(100);
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Cancelled).IsRequired();

            
        }
    }
}
