using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemMap : IEntityTypeConfiguration<SaleItemEntity>
    {
        public void Configure(EntityTypeBuilder<SaleItemEntity> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Discount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Total).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Sale)
                   .WithMany(x => x.Items)
                   .HasForeignKey(x => x.SaleId);
        }
    }

}
