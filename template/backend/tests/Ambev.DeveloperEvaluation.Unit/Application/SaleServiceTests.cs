using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Application.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository> _saleRepo = new();
        private readonly Mock<ISaleItemRepository> _itemRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<ILogger<SaleService>> _logger = new();
        private readonly SaleService _service;

        public SaleServiceTests()
        {
            _service = new SaleService(_saleRepo.Object, _itemRepo.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedSales()
        {
            var sales = new List<SaleEntity> { new SaleEntity() };
            _saleRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(sales);
            _mapper.Setup(m => m.Map<IEnumerable<SaleReadDTO>>(sales)).Returns(new List<SaleReadDTO>());

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedSale()
        {
            var sale = new SaleEntity();
            _saleRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(sale);
            _mapper.Setup(m => m.Map<SaleReadDTO>(sale)).Returns(new SaleReadDTO());

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAsync_ShouldCreateSale_WithoutDiscount()
        {
            var dto = new SalePostDTO
            {
                Items = new List<SaleItemPostDTO> { new() { ProductName = "P", Quantity = 2, UnitPrice = 10m } }
            };

            var entity = new SaleEntity
            {
                Items = new List<SaleItemEntity>
                {
                    new() { ProductName = "P", Quantity = 2, UnitPrice = 10m }
                }
            };

            _mapper.Setup(m => m.Map<SaleEntity>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<SaleReadDTO>(It.IsAny<SaleEntity>())).Returns(new SaleReadDTO());
            _saleRepo.Setup(r => r.AddAsync(It.IsAny<SaleEntity>())).Returns(Task.CompletedTask);
            _saleRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.PostAsync(dto);

            result.Should().NotBeNull();
            entity.Items[0].Discount.Should().Be(0);
        }

        [Fact]
        public async Task PostAsync_ShouldApply10PercentDiscount_WhenQuantityBetween4And9()
        {
            var dto = new SalePostDTO
            {
                Items = new List<SaleItemPostDTO> { new() { ProductName = "P", Quantity = 5, UnitPrice = 10m } }
            };

            var entity = new SaleEntity
            {
                Items = new List<SaleItemEntity>
                {
                    new() { ProductName = "P", Quantity = 5, UnitPrice = 10m }
                }
            };

            _mapper.Setup(m => m.Map<SaleEntity>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<SaleReadDTO>(It.IsAny<SaleEntity>())).Returns(new SaleReadDTO());

            var result = await _service.PostAsync(dto);

            result.Should().NotBeNull();
            entity.Items[0].Discount.Should().Be(5);
        }

        [Fact]
        public async Task PostAsync_ShouldApply20PercentDiscount_WhenQuantityBetween10And20()
        {
            var dto = new SalePostDTO
            {
                ClientId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<SaleItemPostDTO>
                {
                    new SaleItemPostDTO
                    {
                        ProductName = "P",
                        Quantity = 10,
                        UnitPrice = 10m
                    }
                }
            };

            var entity = new SaleEntity
            {
                Items = new List<SaleItemEntity>
                {
                    new() { ProductName = "P", Quantity = 10, UnitPrice = 10m }
                }
            };

            _mapper.Setup(m => m.Map<SaleEntity>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<SaleReadDTO>(It.IsAny<SaleEntity>())).Returns(new SaleReadDTO());

            var result = await _service.PostAsync(dto);

            result.Should().NotBeNull();
            entity.Items[0].Discount.Should().Be(20);
        }

        [Fact]
        public async Task PostAsync_ShouldThrow_WhenQuantityAbove20()
        {
            var dto = new SalePostDTO
            {
                Items = new List<SaleItemPostDTO> { new() { ProductName = "P", Quantity = 21, UnitPrice = 10m } }
            };

            var entity = new SaleEntity
            {
                Items = new List<SaleItemEntity>
                {
                    new() { ProductName = "P", Quantity = 21, UnitPrice = 10m }
                }
            };

            _mapper.Setup(m => m.Map<SaleEntity>(dto)).Returns(entity);

            var act = async () => await _service.PostAsync(dto);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task PutAsync_ShouldUpdateSale()
        {
            var id = Guid.NewGuid();
            var entity = new SaleEntity { Id = id };
            var dto = new SalePutDTO();

            _saleRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mapper.Setup(m => m.Map(dto, entity));
            _mapper.Setup(m => m.Map<SaleReadDTO>(entity)).Returns(new SaleReadDTO());

            var result = await _service.PutAsync(id, dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CancelAsync_ShouldSetCancelledToTrue()
        {
            var id = Guid.NewGuid();
            var entity = new SaleEntity { Id = id, Cancelled = false };

            _saleRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _saleRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.CancelAsync(id);

            result.Should().BeTrue();
            entity.Cancelled.Should().BeTrue();
        }

        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenAlreadyCancelled()
        {
            var id = Guid.NewGuid();
            var entity = new SaleEntity { Id = id, Cancelled = true };

            _saleRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            var act = async () => await _service.CancelAsync(id);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task CancelItemAsync_ShouldSetItemCancelled_AndUpdateSaleTotal()
        {
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var item = new SaleItemEntity
            {
                Id = itemId,
                SaleId = saleId,
                Cancelled = false,
                Total = 50
            };

            var sale = new SaleEntity
            {
                Id = saleId,
                Cancelled = false,
                Items = new List<SaleItemEntity>
                {
                    item,
                    new() { Id = Guid.NewGuid(), Cancelled = false, Total = 30 }
                }
            };

            _saleRepo.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);
            _itemRepo.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            _saleRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.CancelItemAsync(saleId, itemId, new SaleItemCancelDTO());

            result.Should().BeTrue();
            item.Cancelled.Should().BeTrue();
            sale.TotalAmount.Should().Be(30);
        }    
    }    
}

    
 
